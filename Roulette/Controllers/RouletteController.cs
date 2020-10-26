using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Roulette.Models.Responses;
using Roulette.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouletteController : ControllerBase
    {
        private readonly ILogger<RouletteController> _logger;
        private readonly IRouletteService _rouletteService;

        public RouletteController(ILogger<RouletteController> logger, IRouletteService rouletteService)
        {
            _logger = logger;
            _rouletteService = rouletteService;
        }

        [HttpPost]
        public async Task<IActionResult> NewGame()
        {
            Guid gameId;
            try
            {
                gameId = await _rouletteService.CreateNewGame();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred when attempting to create a new game: {ex}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return Ok(new NewGameResponse { GameId = gameId });
        }


        [HttpPost]
        public IActionResult CloseBets()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult AddBet()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveBet()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult Spin()
        {
            return Ok();
        }
    }
}
