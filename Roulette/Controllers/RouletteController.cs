using Microsoft.AspNetCore.Mvc;
using Roulette.Models.Responses;
using Roulette.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Roulette.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouletteController : ControllerBase
    {
        private readonly IGameService _gameService;

        public RouletteController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// Creates new game
        /// </summary>
        /// <returns>OK with NewGameResponse</returns>
        /// <response code="200">OK</response>
        /// <response code="500">Unhandled exception</response>
        [ProducesResponseType(typeof(NewGameResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("new")]
        public async Task<IActionResult> NewGame()
        {
            var response = await _gameService.CreateNewGame();
            return Ok(response);
        }

        /// <summary>
        /// Closes bets for a particular game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>OK</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Game not found</response>
        /// <response code="500">Unhandled exception</response>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("{gameId}/close-betting")]
        public async Task<IActionResult> CloseBetting(Guid gameId)
        {
            await _gameService.CloseBetting(gameId);
            return Ok();
        }

        /// <summary>
        /// Plays a game
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>OK with PlayGameResponse</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Game not found</response>
        /// <response code="500">Unhandled exception</response>
        [ProducesResponseType(typeof(PlayGameResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("{gameId}/play")]
        public async Task<IActionResult> PlayGame(Guid gameId)
        {
            var response = await _gameService.PlayGame(gameId);
            return Ok(response);
        }
    }
}
