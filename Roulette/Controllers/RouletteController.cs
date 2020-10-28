using Microsoft.AspNetCore.Mvc;
using Roulette.Models.Requests;
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
        private readonly IRouletteService _rouletteService;

        public RouletteController(IRouletteService rouletteService)
        {
            _rouletteService = rouletteService;
        }

        /// <summary>
        /// Creates new game
        /// </summary>
        /// <returns>OK with NewGameResponse</returns>
        /// <response code="200">OK</response>
        /// <response code="500">Unhandled exception</response>
        [ProducesResponseType(typeof(NewGameResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("new-game")]
        public async Task<IActionResult> NewGame()
        {
            var response = await _rouletteService.CreateNewGame();
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
        [HttpPost("close-betting/{gameId}")]
        public async Task<IActionResult> CloseBetting(Guid gameId)
        {
            await _rouletteService.CloseBetting(gameId);
            return Ok();
        }

        /// <summary>
        /// Adds a bet for a particular game and customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns>OK</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Game not found</response>
        /// <response code="500">Unhandled exception</response>
        [ProducesResponseType(typeof(AddBetResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("add-bet")]
        public async Task<IActionResult> AddBet(AddBetRequest request)
        {
            var response = await _rouletteService.AddBet(request);
            return Ok(response);
        }

        //TODO update bet

        /// <summary>
        /// Removes a bet
        /// </summary>
        /// <param name="request"></param>
        /// <returns>OK</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Game not found</response>
        /// <response code="500">Unhandled exception</response>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("delete-bet")]
        public async Task<IActionResult> DeleteBet(DeleteBetRequest request)
        {
            await _rouletteService.DeleteBet(request);
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
        [HttpPost("play-game/{gameId}")]
        public async Task<IActionResult> PlayGame(Guid gameId)
        {
            var response = await _rouletteService.PlayGame(gameId);
            return Ok(response);
        }
    }
}
