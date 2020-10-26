using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Roulette.Models.Requests;
using Roulette.Models.Responses;
using Roulette.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var gameId = await _rouletteService.CreateNewGame();

            return Ok(new NewGameResponse { GameId = gameId });
        }

        /// <summary>
        /// Closes bets for a particular game
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
        [HttpPost("close-bets")]
        public IActionResult CloseBets(CloseBetsRequest request)
        {
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("add-bet")]
        public IActionResult AddBet()
        {
            return Ok();
        }

        /// <summary>
        /// Removes a bet for a particular game and customer
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
        [HttpPost("remove-bet")]
        public IActionResult RemoveBet()
        {
            return Ok();
        }

        /// <summary>
        /// Plays a game
        /// </summary>
        /// <param name="request"></param>
        /// <returns>OK with PlayGameResponse</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Game not found</response>
        /// <response code="500">Unhandled exception</response>
        [ProducesResponseType(typeof(PlayGameResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost("play-game")]
        public IActionResult PlayGame(PlayGameRequest request)
        {
            return Ok();
        }
    }
}
