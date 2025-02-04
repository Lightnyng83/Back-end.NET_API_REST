using AutoMapper;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data.Services;


namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("curves")]
    public class CurveController : ControllerBase
    {
        private readonly ICurvePointService _curvePointService;
        private readonly IMapper _mapper;


        public CurveController(ICurvePointService curvePointService, IMapper mapper)
        {
            _curvePointService = curvePointService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CurvePointViewModel curvePoint)
        {
            if(curvePoint == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            CurvePoint curve = _mapper.Map<CurvePoint>(curvePoint);
            await _curvePointService.AddAsync(curve);
            return CreatedAtAction(nameof(Create), new { id = curve.Id }, curve);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEntity(int id)
        {
            var curvePoint = await _curvePointService.FindByIdAsync(id);
            if (curvePoint == null)
            {
                return NotFound($"Curve Point with ID {id} not found.");
            }
            var curvePointViewModel = _mapper.Map<CurvePointViewModel>(curvePoint);

            return Ok(curvePointViewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCurvePoint(int id, [FromBody] CurvePointViewModel curvePointViewModel)
        {
            if (curvePointViewModel == null || id <= 0)
            {
                return BadRequest("Invalid data provided.");
            }

            var existingCurve = await _curvePointService.FindByIdAsync(id);
            if (existingCurve == null)
            {
                return NotFound($"Curve Point with ID {id} not found.");
            }

            _mapper.Map(curvePointViewModel, existingCurve);

            await _curvePointService.UpdateAsync(existingCurve);

            var updatedCurve = _mapper.Map<CurvePointViewModel>(existingCurve);

            return Ok(updatedCurve);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCurvePoint(int id)
        {
            var existingCurve = await _curvePointService.FindByIdAsync(id);
            if (existingCurve == null)
            {
                return NotFound($"Curve Point with ID {id} not found.");
            }

            await _curvePointService.DeleteAsync(existingCurve.Id);

            return NoContent();
        }
    }
}