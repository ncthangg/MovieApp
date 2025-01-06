using Microsoft.AspNetCore.Mvc;
using MovieApp.Common.DTOs.Request;
using MovieApp.Data.Models;
using MovieApp.Service;

namespace MovieApp.API.Controllers
{
    [Route("watchhistory")]
    [ApiController]
    public class UserWatchHistoryController : Controller
    {
        private readonly ServiceWrapper _serviceWrapper;

        public UserWatchHistoryController(ServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }

        [HttpGet("user/progress/{userId}")]
        public async Task<IActionResult> GetAllMovieProgress(long userId)
        {
            var progressData = await _serviceWrapper.UserWatchHistoryService.GetAllMovieProgress(userId);
            return Ok(progressData);
        }

        [HttpGet("user/progress/movie/{userId}/{movieId}")]
        public async Task<IActionResult> GetMovieProgress(long userId, long movieId)
        {
            var progressData = await _serviceWrapper.UserWatchHistoryService.GetMovieProgress(userId, movieId);
            return Ok(progressData);
        }

        [HttpGet("user/progress/season/{userId}/{movieId}/{seasonId}")]
        public async Task<IActionResult> GetSeasonProgress(long userId, long movieId, long seasonId)
        {
            var progressData = await _serviceWrapper.UserWatchHistoryService.GetSeasonProgress(userId, movieId, seasonId);
            return Ok(progressData);
        }

        [HttpGet("user/progress/season/{userId}/{movieId}/{seasonId}/{episodeId}")]
        public async Task<IActionResult> GetEpisodeProgress(long userId, long movieId, long seasonId, long episodeId)
        {
            var progressData = await _serviceWrapper.UserWatchHistoryService.GetEpisodeProgress(userId, movieId, seasonId, episodeId);
            return Ok(progressData);
        }


        [HttpPost("addOrUpdate")]
        public async Task<IActionResult> AddOrUpdateWatchHistory([FromBody] RequestUserWatchHistoryDto request)
        {
            var result = await _serviceWrapper.UserWatchHistoryService.Upsert(request);
            return Ok(result);
        }

    }
}
