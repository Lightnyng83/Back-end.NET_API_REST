using AutoMapper;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Controllers.Model;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data.Services;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;

        public RatingController(IRatingService ratingService, IMapper mapper)
        {
            _ratingService = ratingService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Home()
        {
            var ratings = _ratingService.FindAllAsync();
            var ratingViewModels = _mapper.Map<List<Rating>>(ratings);
            return Ok(ratingViewModels);
        }

        

        [HttpGet]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Create([FromBody]RatingViewModel ratingViewModel)
        {
            if(ratingViewModel == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            Rating rating = _mapper.Map<Rating>(ratingViewModel);
            _ratingService.AddAsync(rating);
            return CreatedAtAction(nameof(Create), new { id = rating.Id }, rating);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetEntity(int id)
        {
            var rating = _ratingService.FindByIdAsync(id);
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


            return Ok(existingRating);
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

            await _ratingService.DeleteAsync(id);

            return NoContent();

        }
    }
}