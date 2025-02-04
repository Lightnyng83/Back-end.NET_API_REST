using AutoMapper;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data.Repositories;
using P7CreateRestApi.Data.Services;
using P7CreateRestApi.Models;

namespace Dot.Net.WebApi.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(IUserService userService, IMapper mapper, IPasswordHasher<IdentityUser> passwordHasher, UserManager<IdentityUser> userManager)
        {
            _userService = userService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
        }


        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Crée un utilisateur
            var user = new IdentityUser
            {
                UserName = model.Username,
                
            };

            // Hache le mot de passe
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            // Sauvegarde l'utilisateur avec UserManager
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                return Ok("User created successfully.");
            }

            // Gestion des erreurs
            return BadRequest(result.Errors);
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