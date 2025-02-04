using AutoMapper;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data.Services;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("trades")]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;
        private readonly IMapper _mapper;

        public TradeController(ITradeService tradeService, IMapper mapper)
        {
            _tradeService = tradeService;
            _mapper = mapper;
        }


        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTrade([FromBody]TradeViewModel tradeViewModel)
        {
            if(tradeViewModel == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            Trade trade = _mapper.Map<Trade>(tradeViewModel);
            await _tradeService.AddTradeAsync(trade);
            return CreatedAtAction(nameof(CreateTrade), new { id = trade.TradeId }, trade);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTrade(int id)
        {
            var trade = await _tradeService.GetTradeByIdAsync(id);
            if (trade == null)
            {
                return NotFound($"Trade with ID {id} not found.");
            }
            var tradeViewModel = _mapper.Map<TradeViewModel>(trade);

            return Ok(tradeViewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateTrade(int id, [FromBody] TradeViewModel tradeViewModel)
        {
            if (tradeViewModel == null || id <= 0)
            {
                return BadRequest();
            }

            var existingTrade = await _tradeService.GetTradeByIdAsync(id);
            if (existingTrade == null)
            {
                return NotFound($"Trade with ID {id} not found.");
            }
            
            _mapper.Map(tradeViewModel, existingTrade);
            await _tradeService.UpdateTradeAsync(existingTrade);

            var updatedTrade = _mapper.Map<TradeViewModel>(existingTrade);
            return Ok(updatedTrade);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            var existingTrade = await _tradeService.GetTradeByIdAsync(id);
            if (existingTrade == null)
            {
                return NotFound($"Trade with ID {id} not found.");
            }
            await _tradeService.DeleteTradeAsync(id);

            return NoContent();
        }
    }
}