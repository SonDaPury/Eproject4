﻿using backend.Base;
using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class SubTopicService(LMSContext context) : ISubTopicService
    {
        private readonly LMSContext _context = context;

        public async Task<SubTopic> CreateAsync(SubTopic subTopic)
        {
            _context.SubTopics.Add(subTopic);
            await _context.SaveChangesAsync();
            return subTopic;
        }

        public Task<List<SubTopic>> GetAllAsync()
        {
            return _context.SubTopics.Include(st => st.Topic).ToListAsync();
        }
        public async Task<(List<SubTopic>,int)> GetAllAsync(Pagination pagination)
        {
            var stps = await _context.SubTopics.Include(st => st.Topic)
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                 .Take(pagination.PageSize)
                .ToListAsync();
            var count = await _context.SubTopics.CountAsync();
            return (stps, count);
        }

        public async Task<SubTopic?> GetByIdAsync(int id)
        {
            return await _context.SubTopics
                .FirstOrDefaultAsync(st => st.Id == id);
        }

        public async Task<SubTopic?> UpdateAsync(int id, SubTopic updatedSubTopic)
        {
            var subTopic = await _context.SubTopics.FindAsync(id);
            if (subTopic == null) return null;

            subTopic.SubTopicName = updatedSubTopic.SubTopicName;
            subTopic.TopicId = updatedSubTopic.TopicId;
            await _context.SaveChangesAsync();
            return subTopic;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subTopic = await _context.SubTopics.FindAsync(id);
            if (subTopic == null) return false;
            var source = await _context.Sources.Where(s => s.SubTopicId == id).ToListAsync();
            if (source != null)
            {
                _context.Sources.RemoveRange(source);
            }
            _context.SubTopics.Remove(subTopic);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
