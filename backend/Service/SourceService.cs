using backend.Base;
using backend.Data;
using backend.Dtos;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using AutoMapper;

namespace backend.Service
{
    public class SourceService(LMSContext context, IMapper mapper, IimageServices imageServices, IExamService examService, IElasticSearchRepository elasticsearchRepository) : ISourceService
    {
        private readonly LMSContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IimageServices _imageServices = imageServices;
        private readonly IExamService _examService = examService;
        private readonly IElasticSearchRepository _elasticsearchRepository = elasticsearchRepository;

        public async Task<Source> CreateAsync(SourceDto sourceDto)
        {
            sourceDto.Slug = GenerateSlug(sourceDto.Title);
            var source = _mapper.Map<Source>(sourceDto);
            // Kiểm tra và xử lý tải lên thumbnail
            if (sourceDto.Thumbnail != null)
            {
                string thumbnailPath = _imageServices.AddFile(sourceDto.Thumbnail, "sources", "thumbnails");
                source.Thumbnail = thumbnailPath;
            }
            // Kiểm tra và xử lý tải lên video giới thiệu
            if (sourceDto.VideoIntro != null)
            {
                string videoPath = _imageServices.AddFile(sourceDto.VideoIntro, "sources", "videos");
                source.VideoIntro = videoPath;
            }
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
                    Id = result.Id,
                    Title = result.Title,
                    Description = result.Description,
                    Thumbnail = result.Thumbnail,
                    Slug = result.Slug,
                    Status = (bool)result.Status? 1 : 0,
                    Video_intro = result.VideoIntro,
                    Price = result.Price,
                    Rating = result.Rating,
                    UserId = (int)result.UserId,
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
            var sources = await _context.Sources
                 //.Include(s => s.User)
                 .Include(s => s.SubTopic)
                 .Select(s => new SourceWithTopicId
                 {
                     Source = _mapper.Map<SourceViewDto>(s),
                     //Source =s,
                     TopicId = s.SubTopic.TopicId ?? null
                 })
                 .ToListAsync();
            return sources;
        }

        public async Task<(List<SourceWithTopicId>, int)> GetAllAsync(Pagination pagination)
        {
            var sources = await _context.Sources
                 //.Include(s => s.User)
                 .Include(s => s.SubTopic)
                 .Select(s => new SourceWithTopicId
                 {
                     Source = _mapper.Map<SourceViewDto>(s),
                     //Source = s,
                     TopicId = s.SubTopic != null ? s.SubTopic.TopicId : null
                 })
                //.Include(s => s.Chapters)
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                 .Take(pagination.PageSize)
                .ToListAsync();
            //var sourceview = _mapper.Map<List<SourceViewDto>>(sources);
            var count = await _context.Sources.CountAsync();
            return (sources, count);
        }

        public async Task<SourceViewDto?> GetByIdAsync(int id)
        {
            var source = await _context.Sources
                //.Include(s => s.User)
                //.Include(s => s.SubTopic)
                //.Select(s => new SourceWithTopicId
                //{
                //    Source = _mapper.Map<SourceViewDto>(s),
                //    TopicId = s.SubTopic.TopicId
                //})
                //.Include(s => s.Chapters)
                .FirstOrDefaultAsync(s => s.Id == id);
            SourceViewDto sourceViewDto = _mapper.Map<SourceViewDto>(source);           

            sourceViewDto.TopicId = source.SubTopic != null ? source.SubTopic.TopicId : null;
            
            return sourceViewDto;
        }

        public async Task<Source?> UpdateAsync(int id, SourceDto updatedSource)
        {
            var source = await _context.Sources.FindAsync(id);
            if (source == null) return null;

            // Cập nhật thông tin từ updatedSource vào source
            source.Title = updatedSource.Title;
            source.Description = updatedSource.Description;
            source.Slug = GenerateSlug(updatedSource.Title);
            source.Status = (bool)updatedSource.Status;
            source.Benefit = updatedSource.Benefit;
            source.Requirement = updatedSource.Requirement;
            source.Price = (double)updatedSource.Price;
            source.Rating = updatedSource.Rating;
            source.UserId = updatedSource.UserId;
            source.SubTopicId = updatedSource.SubTopicId;
            //source.StaticFolder = updatedSource.StaticFolder;

            // Update thumbnail file if a new file is provided
            if (updatedSource.Thumbnail != null && !string.IsNullOrWhiteSpace(updatedSource.Thumbnail.FileName))
            {
                if (!string.IsNullOrWhiteSpace(source.Thumbnail))
                {
                    // Assuming Thumbnail stores a relative path under wwwroot
                    string existingThumbnailPath = Path.Combine("wwwroot", source.Thumbnail);
                    source.Thumbnail = _imageServices.UpdateFile(updatedSource.Thumbnail, existingThumbnailPath, "sources", "thumbnails");
                }
                else
                {
                    source.Thumbnail = _imageServices.AddFile(updatedSource.Thumbnail, "sources", "thumbnails");
                }
            }

            // Update video file if a new file is provided
            if (updatedSource.VideoIntro != null && !string.IsNullOrWhiteSpace(updatedSource.VideoIntro.FileName))
            {
                if (!string.IsNullOrWhiteSpace(source.VideoIntro))
                {
                    // Assuming VideoIntro stores a relative path under wwwroot
                    string existingVideoPath = Path.Combine("wwwroot", source.VideoIntro);
                    source.VideoIntro = _imageServices.UpdateFile(updatedSource.VideoIntro, existingVideoPath, "sources", "videos");
                }
                else
                {
                    source.VideoIntro = _imageServices.AddFile(updatedSource.VideoIntro, "sources", "videos");
                }
            }
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
            //var list_exam = await _context.Exams.Where(l => l.SourceId == id).ToListAsync();
            //if (list_exam != null)
            //{
            //    foreach (var exam in list_exam)
            //    {
            //        await _examService.DeleteAsync(exam.Id);
            //    }
            //    //_context.Exams.RemoveRange(list_exam);
            //}
            // xóa exam theo chapter
            var list_chapter = await _context.Chapters.Where(l => l.SourceId == id).ToListAsync();
            if (list_chapter != null)
            {
                _context.Chapters.RemoveRange(list_chapter);
            }
            if (!string.IsNullOrEmpty(source.Thumbnail))
            {
                _imageServices.DeleteFile(source.Thumbnail);  // Delete thumbnail file
            }
            if (!string.IsNullOrEmpty(source.VideoIntro))
            {
                _imageServices.DeleteFile(source.VideoIntro);  // Delete video intro file
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

        private static string GenerateSlug(string title)
        {
            string normalizedString = title.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new();

            foreach (var c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string cleanStr = stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
            string slug = Regex.Replace(cleanStr, @"\s+", "-"); // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", ""); // Remove all non-alphanumeric characters except hyphens

            return slug;
        }

    }

}
