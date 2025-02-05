using AutoMapper;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Controllers.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data.Services;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("ratings")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;

        public RatingController(IRatingService ratingService, IMapper mapper)
        {
            _ratingService = ratingService;
            _mapper = mapper;
        }



        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] RatingViewModel ratingViewModel)
        {
            if (ratingViewModel == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            Rating rating = _mapper.Map<Rating>(ratingViewModel);
            await _ratingService.AddAsync(rating);
            return CreatedAtAction(nameof(Create), new { id = rating.Id }, rating);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEntity(int id)
        {
            var rating = await _ratingService.FindByIdAsync(id);
            if (rating == null)
            {
                return NotFound($"Rating with ID {id} not found.");
            }
            var ratingViewModel = _mapper.Map<RatingViewModel>(rating);

            return Ok(rating);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRating(int id, [FromBody] RatingViewModel ratingViewModel)
        {
            if (ratingViewModel == null || id <= 0)
            {
                return BadRequest();
            }

            var existingRating = await _ratingService.FindByIdAsync(id);
            if (existingRating == null)
            {
                return NotFound($"Rating with ID {id} not found.");
            }

            _mapper.Map(ratingViewModel, existingRating);

            await _ratingService.UpdateAsync(existingRating);

            var updatedRating = _mapper.Map<RatingViewModel>(existingRating);

            return Ok(updatedRating);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var existingRating = await _ratingService.FindByIdAsync(id);
            if (existingRating == null)
            {
                return NotFound($"Rating with ID {id} not found.");
            }

            await _ratingService.DeleteAsync(existingRating.Id);

            return NoContent();

        }
    }
}