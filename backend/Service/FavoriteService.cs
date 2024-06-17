﻿using backend.Data;
using backend.Entities;
using backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class FavoriteService : IFavoriteService
    {
        private readonly LMSContext _context;
        private readonly IimageServices _imageServices;
        public FavoriteService(LMSContext context, IimageServices imageServices)
        {
            _context = context;
            _imageServices = imageServices;
        }
        public async Task<FavoriteSource> AddFavorite(int userId, int sourceId)
        {
            var favorite = new FavoriteSource
            {
                UserId = userId,
                SourceId = sourceId,
                IsFavorite = true
            };
            await _context.FavoriteSources.AddAsync(favorite);
            await _context.SaveChangesAsync();
            return favorite;
        }
        public async Task UnFavorite(int favoriteId)
        {
            var favorite = await _context.FavoriteSources.FindAsync(favoriteId);
            if (favorite == null)
            {
                throw new Exception("not found cource favorite");
            }
            _context.FavoriteSources.Remove(favorite);
            await _context.SaveChangesAsync();  
        }
        public async Task<List<Source>> GetSourcesFavoriteByUserId(int userId)
        {
            var sources = await _context.Sources
                .Include(s => s.FavoriteSources)
                .Where(s => s.FavoriteSources.Any(f => f.UserId == userId))
                .ToListAsync();
            if (sources.Count != 0)
                foreach (var source in sources)
                {
                    if (source.Thumbnail != null)
                        source.Thumbnail = _imageServices.GetFile(source.Thumbnail);
                    if (source.VideoIntro != null)
                        source.VideoIntro = _imageServices.GetFile(source.VideoIntro);
                }
            return sources;
        }
        public async Task<List<Source>> GetTop5FavoriteSources()
        {
            var topSources = await _context.Sources
                .Include(s => s.FavoriteSources)
                .OrderByDescending(s => s.FavoriteSources.Count)
                .Take(5)
                .ToListAsync();
            if (topSources.Count != 0)
                foreach (var source in topSources)
                {
                    if (source.Thumbnail != null)
                        source.Thumbnail = _imageServices.GetFile(source.Thumbnail);
                    if (source.VideoIntro != null)
                        source.VideoIntro = _imageServices.GetFile(source.VideoIntro);
                }
            return topSources;
        }
    }
}
