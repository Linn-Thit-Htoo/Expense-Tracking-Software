using ExpenseTrackerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers
{
    public class EncryptController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EncryptService _encryptService;

        public EncryptController(IConfiguration configuration, EncryptService encryptService)
        {
            _configuration = configuration;
            _encryptService = encryptService;
        }

        [HttpGet]
        [Route("/api/encrypt")]
        public IActionResult Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return BadRequest();

            string encryptedText = _encryptService.EncryptString(str, _configuration["EncryptionKey"]!);
            return Ok(encryptedText);
        }
    }
}
