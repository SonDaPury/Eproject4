using AutoMapper;
using backend.Dtos;
using backend.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteSourceController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IMapper _mapper;
        public FavoriteSourceController(IFavoriteService favoriteService, IMapper mapper)
        {
            _favoriteService = favoriteService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> AddFavorite(int userId, int sourceId)
        {
            try
            {
                var favorite = await _favoriteService.AddFavorite(userId, sourceId);
                return Ok(favorite);
            }
            catch(Exception ex) 
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> UnFavorite (int favoriteId)
        {
            try
            {
                await _favoriteService.UnFavorite(favoriteId);
                return Ok("Unfavorite successfully !!!");
            }catch(Exception ex)
            {
                return BadRequest(new {message =  ex.Message});
            }
        }
        [HttpGet("getsourcefavoritebyuserid/{userId}")] 
        public async Task<IActionResult> GetSourceFavoriteByUserId (int userId)
        {
            try
            {
                var list = await _favoriteService.GetSourcesFavoriteByUserId(userId);
                var listDto = _mapper.Map<List<SourceViewDto>>(list);
                return Ok(new { UserId = userId , Sources = listDto });
            }catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("gettop5favorite")]
        public async Task<IActionResult> GetTop5Source()
        {
            try
            {
                var list = await _favoriteService.GetTop5FavoriteSources();
                var listDto = _mapper.Map<List<SourceViewDto>>(list);
                return Ok(listDto);
            }catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}
