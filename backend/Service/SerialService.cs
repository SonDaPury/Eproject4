using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class SerialService(LMSContext context) : ISerialService
    {
        private readonly LMSContext _context = context;

        public async Task<Serial> CreateAsync(Serial newSerial)
        {
            // Xác định index lớn nhất hiện có
            int? maxIndex = await _context.Serials.MaxAsync(s => (int?)s.Index);

            // Nếu chưa có Serial nào, gán index là 0
            if (maxIndex == null)
            {
                newSerial.Index = 0;
            }
            else
            {
                // Kiểm tra xem có cần phải cập nhật index của các Serial khác không
                if (newSerial.Index <= maxIndex)
                {
                    // Tăng index cho tất cả các Serial có index >= giá trị index mới
                    var serialsToUpdate = _context.Serials.Where(s => s.Index >= newSerial.Index);
                    foreach (var serial in serialsToUpdate)
                    {
                        serial.Index++;
                    }
                }
                else
                {
                    // Nếu thêm vào cuối, chỉ cần gán index là maxIndex + 1
                    newSerial.Index = maxIndex.Value + 1;
                }
            }

            _context.Serials.Add(newSerial);
            await _context.SaveChangesAsync();
            return newSerial;
        }

        public async Task<List<Serial>> GetAllAsync()
        {
            return await _context.Serials.ToListAsync();
        }

        public async Task<Serial?> GetByIdAsync(int id)
        {
            return await _context.Serials
                .FirstOrDefaultAsync(st => st.Id == id);
        }

        public async Task<Serial?> UpdateAsync(int id, Serial updatedSerial)
        {
            var serial = await _context.Serials.FindAsync(id);
            if (serial == null) return null;

            if (serial.Index != updatedSerial.Index)
            {
                serial.Index = updatedSerial.Index;
                serial.LessonId = updatedSerial.LessonId;
                serial.ExamId = updatedSerial.ExamId;

                // Determine the range of indices to update
                int minIndex = Math.Min(serial.Index, updatedSerial.Index);
                int maxIndex = Math.Max(serial.Index, updatedSerial.Index);

                var serialsToUpdate = _context.Serials
                    .Where(s => s.Index >= minIndex && s.Index <= maxIndex && s.Id != id);

                if (serial.Index > updatedSerial.Index)
                {
                    await serialsToUpdate.ForEachAsync(s => s.Index++);
                }
                else
                {
                    await serialsToUpdate.ForEachAsync(s => s.Index--);
                }
            }
            await _context.SaveChangesAsync();
            return serial;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var serial = await _context.Serials.FindAsync(id);
                    if (serial == null) return false;

                    _context.Serials.Remove(serial);

                    // Efficiently update all higher index Serials in one go if supported
                    var higherIndexSerials = _context.Serials.Where(s => s.Index > serial.Index);
                    foreach (var higherSerial in higherIndexSerials)
                    {
                        higherSerial.Index--;
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return true;
                }
                catch (Exception ex)
                {
                    // Rollback transaction and handle or log the exception
                    await transaction.RollbackAsync();
                    // Log the exception (pseudo-code)
                    // Log.Error("Failed to delete and re-index Serials", ex);
                    return false;
                }
            }
        }

        //public async Task UpdateIndexHightoLow (int index, int indexhigher)
        //{
        //    var ListSerial = await _context.Serials.Where(s => s.Index > index && s.Index < indexhigher).ToListAsync();
        //    foreach (Serial serial in ListSerial)
        //    {
        //        serial.Index++;
        //    }
        //    await _context.SaveChangesAsync();
        //}
        //public async Task UpdateIndexLowtoHigh(int index, int indexhigher)
        //{
        //    var ListSerial = await _context.Serials.Where(s => s.Index > index && s.Index < indexhigher).ToListAsync();
        //    foreach (Serial serial in ListSerial)
        //    {
        //        serial.Index--;
        //    }
        //    await _context.SaveChangesAsync();
        //}
        //public async Task InsertIndex(int index)
        //{
        //    var ListSerial = await _context.Serials.Where(s => s.Index > index).ToListAsync();
        //    foreach (Serial serial in ListSerial)
        //    {
        //        serial.Index--;
        //    }
        //    await _context.SaveChangesAsync();
        //}
    }
}
