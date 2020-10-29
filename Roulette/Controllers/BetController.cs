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
    [Route("api/roulette/{gameId}/bet")]
    public class BetController : ControllerBase
    {
        private readonly IBetService _betService;

        public BetController(IBetService betService)
        {
            _betService = betService;
        }

        /// <summary>
        /// Adds a bet for a particular game and customer
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="request"></param>
        /// <returns>OK with AddBetResponse</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Game not found</response>
        /// <response code="500">Unhandled exception</response>
        [ProducesResponseType(typeof(AddBetResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> AddBet([FromRoute] Guid gameId, [FromBody] AddBetRequest request)
        {
            var response = await _betService.AddBet(gameId, request);
            return Ok(response);
        }

        /// <summary>
        /// Updates a bet
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="betId"></param>
        /// <param name="request"></param>
        /// <returns>OK</returns>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Game not found</response>
        /// <response code="500">Unhandled exception</response>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut("{betId}")]
        public async Task<IActionResult> UpdateBet([FromRoute] Guid gameId, [FromRoute] Guid betId, [FromBody] UpdateBetRequest request)
        {
            await _betService.UpdateBet(gameId, betId, request);
            return NoContent();
        }

        /// <summary>
        /// Removes a bet
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="betId"></param>
        /// <returns>OK</returns>
        /// <response code="204">OK</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Game/Bet not found</response>
        /// <response code="500">Unhandled exception</response>
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{betId}")]
        public async Task<IActionResult> DeleteBet([FromRoute] Guid gameId, [FromRoute] Guid betId)
        {
            await _betService.DeleteBet(gameId, betId);
            return NoContent();
        }
    }
}
