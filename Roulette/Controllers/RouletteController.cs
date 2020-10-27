using Microsoft.AspNetCore.Mvc;
using Roulette.Models.Requests;
using Roulette.Models.Responses;
using Roulette.Services;
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
        public async Task<IActionResult> CloseBets(CloseBetsRequest request)
        {
            await _rouletteService.CloseBets(request);

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
        public async Task<IActionResult> RemoveBet(RemoveBetRequest request)
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
        public async Task<IActionResult> PlayGame(PlayGameRequest request)
        {
            return Ok();
        }
    }
}
