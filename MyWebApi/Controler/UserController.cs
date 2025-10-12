using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Data;
using MyWebApi.Model;
using MyWebApi.Models;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.User.ToListAsync();
            return Ok(users);
        }

        // POST: api/user
        [HttpPost("Create user")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return BadRequest(new { message = "Login i hasło są wymagane." });
            }

            // Sprawdź, czy login już istnieje
            if (await _context.User.AnyAsync(u => u.Login == user.Login))
            {
                return Conflict(new { message = "Użytkownik o takim loginie już istnieje." });
            }

            // 🔒 Hashowanie hasła
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            // 🆕 Ustawienie roli (jeśli nie podano, będzie 'User')
            if (string.IsNullOrEmpty(user.Role))
                user.Role = "User";

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, new
            {
                user.Id,
                user.Login,
                user.Role
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser request)
        {
            if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Login i hasło są wymagane." });
            }
            var user = await _context.User.FirstOrDefaultAsync(u => u.Login == request.Login);
            if (user == null)
            {
                return Unauthorized(new { message = "Nieprawidłowy login lub hasło." });
            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return Unauthorized(new { message = "Nieprawidłowy login lub hasło." });
            }
            return Ok(new
            {
                message = "Zalogowano pomyślnie ✅",
                user = new { user.Id, user.Login }
            });
        }

        //put
        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Użytkownik nie znaleziony." });

            // 🧠 Walidacja loginu
            if (!string.IsNullOrWhiteSpace(updatedUser.Login))
                user.Login = updatedUser.Login;

            // 🔒 Jeśli użytkownik poda nowe hasło, zhashuj je
            if (!string.IsNullOrWhiteSpace(updatedUser.PasswordHash))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUser.PasswordHash);

            // 👑 Jeśli poda nową rolę — zaktualizuj
            if (!string.IsNullOrWhiteSpace(updatedUser.Role))
                user.Role = updatedUser.Role;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Dane użytkownika zostały zaktualizowane ✅",
                user = new
                {
                    user.Id,
                    user.Login,
                    user.Role
                }
            });
        }


        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Użytkownik nie znaleziony." });
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Użytkownik został usunięty ✅" });
        }

    }
}
