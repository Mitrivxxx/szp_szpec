using AutoMapper;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Data;
using MyWebApi.DTOs.Users;
using MyWebApi.Model;
using MyWebApi.Services.Interface;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        // Dependency Injection - wstrzykujemy serwis zamiast DbContext
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Użytkownik nie znaleziony." });

            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (string.IsNullOrWhiteSpace(createUserDto.Login) ||
                string.IsNullOrWhiteSpace(createUserDto.Password))
            {
                return BadRequest(new { message = "Login i hasło są wymagane." });
            }

            try
            {
                var user = await _userService.CreateUserAsync(createUserDto);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new
                {
                    message = "Użytkownik został utworzony ✅",
                    user
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Login) ||
                string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest(new { message = "Login i hasło są wymagane." });
            }

            var user = await _userService.LoginAsync(loginDto);
            if (user == null)
            {
                return Unauthorized(new { message = "Nieprawidłowy login lub hasło." });
            }

            return Ok(new
            {
                message = "Zalogowano pomyślnie ✅",
                user
            });
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            if (user == null)
                return NotFound(new { message = "Użytkownik nie znaleziony." });

            return Ok(new
            {
                message = "Dane użytkownika zostały zaktualizowane ✅",
                user
            });
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
                return NotFound(new { message = "Użytkownik nie znaleziony." });

            return Ok(new { message = "Użytkownik został usunięty ✅" });
        }
    }
}