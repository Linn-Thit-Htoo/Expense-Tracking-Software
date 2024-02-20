using ExpenseTrackerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers
{
    public class DecryptController : ControllerBase
    {
        private readonly DecryptService _decryptService;
        private readonly IConfiguration _configuration;

        public DecryptController(DecryptService decryptService, IConfiguration configuration)
        {
            _decryptService = decryptService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/api/decrypt")]
        public IActionResult Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return BadRequest();

            string decryptedStr = _decryptService.DecryptString(str, _configuration["EncryptionKey"]!);
            return Ok(decryptedStr);
        }
    }
}
