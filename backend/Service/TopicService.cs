using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class TopicService(LMSContext context) : ITopicService
    {
        private readonly LMSContext _context = context;

        // Tạo mới một topic
        public async Task<Topic> CreateAsync(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
            return topic;
        }

        // Lấy danh sách tất cả topics
        public async Task<List<Topic>> GetAllAsync()
        {
            return await _context.Topics.ToListAsync();
        }

        // Lấy thông tin một topic theo ID
        public async Task<Topic?> GetByIdAsync(int id)
        {
            return await _context.Topics
                .Include(t => t.subTopics)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Cập nhật thông tin một topic
        public async Task<Topic?> UpdateAsync(int id, Topic updatedItem)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null) return null;

            topic.TopicName = updatedItem.TopicName;
            await _context.SaveChangesAsync();
            return topic;
        }

        // Xóa một topic
        public async Task<bool> DeleteAsync(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null) return false;
            var sub_topic = await _context.SubTopics.Where(t => t.TopicId == id).ToListAsync();
            if (sub_topic != null)
            {
                _context.SubTopics.RemoveRange(sub_topic);
            }
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
