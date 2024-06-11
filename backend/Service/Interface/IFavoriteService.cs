using backend.Entities;

namespace backend.Service.Interface
{
    public interface IFavoriteService
    {
        Task<FavoriteSource> AddFavorite(int userId, int sourceId);
        Task<List<Source>> GetSourcesFavoriteByUserId(int userId);
        Task<List<Source>> GetTop5FavoriteSources();
        Task UnFavorite(int id);
    }
}
