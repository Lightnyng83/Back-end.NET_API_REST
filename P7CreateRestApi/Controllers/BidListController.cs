using AutoMapper;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data.Services;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("bidlists")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BidListController : ControllerBase
    {
        private readonly IBidListService _bidListService;
        private readonly IMapper _mapper;

        public BidListController(IBidListService bidListService, IMapper mapper)
        {
            _bidListService = bidListService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] BidListViewModel bidListViewModel)
        {
            if (bidListViewModel == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            BidList bidList = _mapper.Map<BidList>(bidListViewModel);

            await _bidListService.AddAsync(bidList);

            return CreatedAtAction(nameof(Create), new { id = bidList.BidListId }, bidList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ReadEntity(int id)
        {
            var bidList = await _bidListService.FindByIdAsync(id);
            if (bidList == null)
            {
                return NotFound($"Bid with ID {id} not found.");
            }
            var bidListViewModel = _mapper.Map<BidListViewModel>(bidList);

            return Ok(bidListViewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateBid(int id, [FromBody] BidListViewModel bidList)
        {
            if (bidList == null || id <= 0)
            {
                return BadRequest("Invalid data provided.");
            }

            var existingBid = await _bidListService.FindByIdAsync(id);
            if (existingBid == null)
            {
                return NotFound($"Bid with ID {id} not found.");
            }

            _mapper.Map(bidList, existingBid);

            await _bidListService.UpdateAsync(existingBid);


            var updatedBid = _mapper.Map<BidListViewModel>(existingBid);

            return Ok(updatedBid);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteBid(int id)
        {
            var existingBid = await _bidListService.FindByIdAsync(id);
            if (existingBid == null)
            {
                return NotFound($"Bid with ID {id} not found.");
            }

            await _bidListService.DeleteAsync(existingBid.BidListId);

            return NoContent();
        }

    }
}