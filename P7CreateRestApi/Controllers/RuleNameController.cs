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
    [Route("rulenames")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RuleNameController : ControllerBase
    {
        private readonly IRuleNameService _ruleNameService;
        private readonly IMapper _mapper;

        public RuleNameController(IRuleNameService ruleNameService, IMapper mapper)
        {
            _ruleNameService = ruleNameService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] RuleNameViewModel tradeViewModel)
        {
            if (tradeViewModel == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            RuleName ruleName = _mapper.Map<RuleName>(tradeViewModel);
            await _ruleNameService.AddAsync(ruleName);
            return CreatedAtAction(nameof(Create), new { id = ruleName.Id }, ruleName);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEntity(int id)
        {
            var ruleName = await _ruleNameService.FindByIdAsync(id);
            if (ruleName == null)
            {
                return NotFound($"RuleName with ID {id} not found.");
            }

            var ruleNameViewModel = _mapper.Map<RuleNameViewModel>(ruleName);

            return Ok(ruleNameViewModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRuleName(int id, [FromBody] RuleNameViewModel ratingViewModel)
        {
            if (ratingViewModel == null || id <= 0)
            {
                return BadRequest();
            }

            var existingRuleName = await _ruleNameService.FindByIdAsync(id);
            if (existingRuleName == null)
            {
                return NotFound($"RuleName with ID {id} not found.");
            }

            _mapper.Map(ratingViewModel, existingRuleName);
            await _ruleNameService.UpdateAsync(existingRuleName);

            var updatedRuleName = _mapper.Map<RuleNameViewModel>(existingRuleName);
            return Ok(updatedRuleName);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRuleName(int id)
        {
            var existingRuleName = await _ruleNameService.FindByIdAsync(id);
            if (existingRuleName == null)
            {
                return NotFound($"RuleName with ID {id} not found.");
            }

            await _ruleNameService.DeleteAsync(existingRuleName.Id);
            return Ok();
        }
    }
}