using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Data;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class dbTest : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<dbTest> _logger;

        public dbTest(AppDbContext context, ILogger<dbTest> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/dbtest
        [HttpGet]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                // Próba prostego zapytania do bazy
                var canConnect = await _context.Database.CanConnectAsync();

                if (canConnect)
                {
                    _logger.LogInformation("✅ Połączenie z bazą danych działa poprawnie.");
                    return Ok(new
                    {
                        status = "success",
                        message = "Połączenie z bazą danych działa poprawnie ✅"
                    });
                }
                else
                {
                    _logger.LogWarning("⚠️ Nie udało się nawiązać połączenia z bazą danych.");
                    return StatusCode(500, new
                    {
                        status = "error",
                        message = "Nie udało się nawiązać połączenia z bazą danych ❌"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas testowania połączenia z bazą danych.");
                return StatusCode(500, new
                {
                    status = "error",
                    message = "Wystąpił błąd podczas testowania połączenia z bazą danych.",
                    details = ex.Message
                });
            }
        }
    }
}
