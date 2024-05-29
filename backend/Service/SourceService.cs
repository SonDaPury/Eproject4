using backend.Base;
using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using backend.Dtos;
using AutoMapper;

namespace backend.Service
{
    public class SourceService(LMSContext context, IMapper mapper) : ISourceService
    {
        private readonly LMSContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<Source> CreateAsync(SourceDto sourceDto)
        {
            sourceDto.Slug = GenerateSlug(sourceDto.Title);
            var source = _mapper.Map<Source>(sourceDto);
            _context.Sources.Add(source);
            await _context.SaveChangesAsync();
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

            source.Title = updatedSource.Title;
            source.Description = updatedSource.Description;
            source.Thumbnail = updatedSource.Thumbnail;
            source.Slug = GenerateSlug(updatedSource.Title);
            source.Status = updatedSource.Status;
            source.Benefit = updatedSource.Benefit;
            source.Requirement = updatedSource.Requirement;
            source.VideoIntro = updatedSource.VideoIntro;
            source.Price = updatedSource.Price;
            source.Rating = updatedSource.Rating;
            source.UserId = updatedSource.UserId;
            source.SubTopicId = updatedSource.SubTopicId;
            //source.StaticFolder = updatedSource.StaticFolder;

            await _context.SaveChangesAsync();
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
            await _context.SaveChangesAsync();
            return true;
        }

        Task<(List<Source>, int)> IService<Source>.GetAllAsync(Pagination pagination)
        {
            throw new NotImplementedException();
        }

        Task<Source?> IService<Source>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<Source>> IService<Source>.GetAllAsync()
        {
            throw new NotImplementedException();
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

        public Task<Source> CreateAsync(Source chapter)
        {
            throw new NotImplementedException();
        }
    }

}
