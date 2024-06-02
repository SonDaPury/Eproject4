using backend.Dtos;
using backend.Entities;

namespace backend.Service.Interface
{
    public interface ISerialService : IService<Serial>
    {
        //Task InsertIndex(int index);
        //Task UpdateIndexHightoLow(int index, int indexhigher);
        Task<List<Serial>> GetAllAsync();
        Task<Serial> CreateSerial(SerialDto chapter);
        Task<Serial?> UpdateSerial(int id, UpdateSerialDto updateSerial);
    }
}
