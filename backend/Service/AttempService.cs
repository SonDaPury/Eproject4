using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class AttempService : IAttempService
    {
        private readonly LMSContext _context;

        public AttempService(LMSContext context)
        {
            _context = context;
        }

        public async Task<Attemp> CreateAsync(Attemp attempt)
        {
            _context.Attemps.Add(attempt);
            await _context.SaveChangesAsync();
            return attempt;
        }

        public async Task<List<Attemp>> GetAllAsync()
        {
            return await _context.Attemps
                //.Include(a => a.User) // Includes the User associated with the Attemp
                //.Include(a => a.Answers) // Includes Answers related to the Attemp
                .ToListAsync();
        }

        public async Task<Attemp?> GetByIdAsync(int id)
        {
            return await _context.Attemps
                //.Include(a => a.User) // Include the User for full context
                //.Include(a => a.Answers) // Include Answers to provide a complete view of the Attemp
            .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Attemp?> UpdateAsync(int id, Attemp updatedAttemp)
        {
            var attempt = await _context.Attemps.FindAsync(id);
            if (attempt == null) return null;

            attempt.Index = updatedAttemp.Index;
            attempt.TimeTaken = updatedAttemp.TimeTaken;
            attempt.UserId = updatedAttemp.UserId;

            await _context.SaveChangesAsync();
            return attempt;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var attempt = await _context.Attemps.FindAsync(id);
            if (attempt == null) return false;
            var answer = await _context.Answers.Where(a => a.ExamId == id).ToListAsync();
            if (answer != null)
            {
                _context.Answers.RemoveRange(answer);
            }
            _context.Attemps.Remove(attempt);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
