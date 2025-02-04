using AutoMapper;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Model;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data.Repositories;
using P7CreateRestApi.Data.Services;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] UserViewModel user)
        {
            if (user == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            User newUser = _mapper.Map<User>(user);

            await _userService.AddAsync(newUser);

            return CreatedAtAction(nameof(Create), new { id = newUser.Id }, newUser);
        }



        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult GetEntity(int id)
        {
            var user = _userService.FindByIdAsync(id).Result;
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var userViewModel = _mapper.Map<UserViewModel>(user);

            return Ok(userViewModel);

        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserViewModel user)
        {
            if (user == null || id <= 0)
            {
                return BadRequest();
            }

            var existingUser = await _userService.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            _mapper.Map(user, existingUser);
            await _userService.UpdateAsync(existingUser);

            var updatedUser = _mapper.Map<UserViewModel>(existingUser);

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _userService.FindByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            await _userService.DeleteAsync(id);     

            return NoContent();
        }

        
    }
}