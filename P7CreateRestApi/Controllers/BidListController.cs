using AutoMapper;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Services;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        [Route("validate")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Validate([FromBody] BidList bidList)
        {
            // TODO: check data valid and save to db, after saving return bid list
            if (bidList == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            _bidListService.Add(bidList);
            var bidLists = await _bidListService.FindAll();
            return CreatedAtAction(nameof(Validate), new { id = bidList.BidListId }, bidLists);
        }

        [HttpGet]
        [Route("update/{id}")]
        public IActionResult ShowUpdateForm(int id)
        {
            return Ok();
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult UpdateBid(int id, [FromBody] BidList bidList)
        {
            // TODO: check required fields, if valid call service to update Bid and return list Bid
            if (bidList == null || id <= 0)
            {
                return BadRequest("Invalid data provided.");
            }

            var existingBid = _bidListService.FindById(id); // Récupérer l'existant
            if (existingBid == null)
            {
                return NotFound($"Bid with ID {id} not found.");
            }

            // Mise à jour des champs
            _mapper.Map(bidList, existingBid);
            
            // Appel du service pour mettre à jour
            _bidListService.Update(existingBid);
            _bidListService.FindAll();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult DeleteBid(int id)
        {
            _bidListService.Delete(id);
            return NoContent();
        }
    }
}