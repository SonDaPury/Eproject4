using backend.Base;
using backend.Data;
using backend.Dtos;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Nest;
using System.Reflection.Metadata;

namespace backend.Service
{
    public class SourceService(LMSContext context, IElasticSearchRepository elasticsearchRepository) : ISourceService
    {
        private readonly LMSContext _context = context;
        private readonly IElasticSearchRepository _elasticsearchRepository = elasticsearchRepository;

        public async Task<Source> CreateAsync(Source source)
        {
            _context.Sources.Add(source);
            try
            {
                int check = await _context.SaveChangesAsync();
                if (check <= 0)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            var result = await GetByIdAsync(source.Id);

            var scirptSource = @"for (int i = 0; i < ctx._source.subTopics.size(); i++) { if (ctx._source.subTopics[i].SubTopicId == params.SubTopicId) { ctx._source.subTopics[i].sources.add(params.sources); } }";
            var scriptParams = new Dictionary<string, object>
{
    { "SubTopicId", source.SubTopicId },
                {"sources", new SourcesElasticSearch
                {
                    Id = result.Source.Id,
                    Title = result.Source.Title,
                    Description = result.Source.Description,
                    Thumbnail = result.Source.Thumbnail,
                    Slug = result.Source.Slug,
                    Status = result.Source.Status? 1 : 0,
                    Video_intro = result.Source.VideoIntro,
                    Price = result.Source.Price,
                    Rating = result.Source.Rating,
                    UserId = result.Source.UserId,
                }
                }

};
            var CreateResponse = _elasticsearchRepository.UpdateScript(result.TopicId.ToString(), u => u
 .Index("sources_index")
 .Script(s => s
     .Source(scirptSource)
     .Params(scriptParams)
 )
);
            return source;
        }

        public async Task<List<SourceWithTopicId>> GetAllAsync()
        {
            return await _context.Sources
                 //.Include(s => s.User)
                 .Include(s => s.SubTopic)
                 .Select(s => new SourceWithTopicId
                 {
                     Source = s,
                     TopicId = s.SubTopic.TopicId
                 })
                 .ToListAsync();
        }

        public async Task<(List<SourceWithTopicId>, int)> GetAllAsync(Pagination pagination)
        {
            var sources = await _context.Sources
                 //.Include(s => s.User)
                 .Include(s => s.SubTopic)
                 .Select(s => new SourceWithTopicId
                 {
                     Source = s,
                     TopicId = s.SubTopic.TopicId
                 })
                //.Include(s => s.Chapters)
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                 .Take(pagination.PageSize)
                .ToListAsync();
            var count = await _context.Sources.CountAsync();
            return (sources, count);
        }

        public async Task<SourceWithTopicId?> GetByIdAsync(int id)
        {
            return await _context.Sources
                //.Include(s => s.User)
                .Include(s => s.SubTopic)
                .Select(s => new SourceWithTopicId
                {
                    Source = s,
                    TopicId = s.SubTopic.TopicId
                })
                //.Include(s => s.Chapters)
                .FirstOrDefaultAsync(s => s.Source.Id == id);
        }

        public async Task<Source?> UpdateAsync(int id, Source updatedSource)
        {
            var source = await _context.Sources.FindAsync(id);
            if (source == null) return null;

            // Cập nhật thông tin từ updatedSource vào source
            source.Title = updatedSource.Title;
            source.Description = updatedSource.Description;
            source.Thumbnail = updatedSource.Thumbnail;
            source.Slug = updatedSource.Slug;
            source.Status = updatedSource.Status;
            source.Benefit = updatedSource.Benefit;
            source.Requirement = updatedSource.Requirement;
            source.VideoIntro = updatedSource.VideoIntro;
            source.Price = updatedSource.Price;
            source.Rating = updatedSource.Rating;
            source.UserId = updatedSource.UserId;
            source.SubTopicId = updatedSource.SubTopicId;
            source.StaticFolder = updatedSource.StaticFolder;

            // Lưu các thay đổi vào cơ sở dữ liệu
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                // Lấy thông tin về Source đã được cập nhật từ cơ sở dữ liệu
                var result = await GetByIdAsync(source.Id);

                var scriptSource = @"
        for (int i = 0; i < ctx._source.subTopics.size(); i++) {
            if (ctx._source.subTopics[i].SubTopicId == params.SubTopicId) {
                for (int j = 0; j < ctx._source.subTopics[i].sources.size(); j++) {
                    if (ctx._source.subTopics[i].sources[j].Id == params.id) {
                        ctx._source.subTopics[i].sources[j].Title = params.Title;
                        ctx._source.subTopics[i].sources[j].Description = params.Description;
                        ctx._source.subTopics[i].sources[j].Thumbnail = params.Thumbnail;
                        ctx._source.subTopics[i].sources[j].Slug = params.Slug;
                        ctx._source.subTopics[i].sources[j].Status = params.Status;
                        ctx._source.subTopics[i].sources[j].Video_intro = params.Video_intro;
                        ctx._source.subTopics[i].sources[j].Price = params.Price;
                        ctx._source.subTopics[i].sources[j].Rating = params.Rating;
                        ctx._source.subTopics[i].sources[j].UserId = params.UserId;
                        break;
                    }
                }
                break;
            }
        }
    ";
                var scriptParams = new Dictionary<string, object>
    {
        { "SubTopicId", source.SubTopicId },
        { "id", source.Id },
        { "Title", source.Title },
        { "Description", source.Description },
        { "Thumbnail", source.Thumbnail },
        { "Slug", source.Slug },
        { "Status", source.Status ? 1 : 0 },
        { "Video_intro", source.VideoIntro },
        { "Price", source.Price },
        { "Rating", source.Rating },
        { "UserId", source.UserId }
    };

                // Cập nhật thông tin trong Elasticsearch
                var updateResponse = _elasticsearchRepository.UpdateScript(result.TopicId.ToString(), u => u
                    .Index("sources_index")
                    .Script(s => s
                        .Source(scriptSource)
                        .Params(scriptParams)
                    )
                );
            }
            return source;
        }


        public async Task<bool> DeleteAsync(int id)
        {

            var source = await _context.Sources.FindAsync(id);
           
            if (source == null) return false;
            // xóa exam theo source
            var list_exam = await _context.Exams.Where(l => l.SourceId == id).ToListAsync();
            if (list_exam != null)
            {
                _context.Exams.RemoveRange(list_exam);
            }
            // xóa exam theo chapter
            var list_chapter = await _context.Chapters.Where(l => l.SourceId == id).ToListAsync();
            if (list_chapter != null)
            {
                _context.Chapters.RemoveRange(list_chapter);
            }
            _context.Sources.Remove(source);
            int check = await _context.SaveChangesAsync();
            if (check > 0)
            {
                 var result = await GetByIdAsync(source.Id);
                var scriptSource = @"ctx._source.subTopics.forEach(subTopic -> { subTopic.sources.removeIf(source -> source.Id == params.id); })";
                var updateResponse = _elasticsearchRepository.UpdateScript(result.TopicId.ToString(), u => u
                       .Index("sources_index")
                       .Script(s => s
                           .Source(scriptSource)
                           .Params(p => p.Add("id", id))
                       )
                   );
                return updateResponse;
            }

            return false;
        }

        Task<(List<Source>, int)> IService<Source>.GetAllAsync(Pagination pagination)
        {
            throw new NotImplementedException();
        }

        Task<List<Source>> IService<Source>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Source?> IService<Source>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }

}
