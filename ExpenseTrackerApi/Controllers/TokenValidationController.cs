using ExpenseTrackerApi.Middleware;
using ExpenseTrackerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpenseTrackerApi.Controllers
{
    public class TokenValidationController : ControllerBase
    {
        private readonly CheckMiddlewareClaimsService _checkMiddlewareClaimsService;
        private readonly DecryptService _decryptService;
        private readonly IConfiguration _configuration;
        public TokenValidationController(CheckMiddlewareClaimsService checkMiddlewareClaimsService, DecryptService decryptService, IConfiguration configuration)
        {
            _checkMiddlewareClaimsService = checkMiddlewareClaimsService;
            _decryptService = decryptService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("api/jwt")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> ValidateJwtToken([FromHeader] HttpContext context)
        {
            try
            {
                string privateKey = _configuration["EncryptionKey"]!;
                string authHeader = context.Request.Headers["Authorization"]!;

                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized("Invalid Token");
                }

                string token = authHeader.Substring("Bearer ".Length);
                string decryptedToken = _decryptService.DecryptString(token, privateKey);
                JwtSecurityTokenHandler tokenHandler = new();
                byte[] key = Encoding.ASCII.GetBytes(privateKey);

                TokenValidationParameters parameters = new()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(decryptedToken, parameters, out SecurityToken securityToken);

                // test role is admin and phone exists and not null for active admin
                if (principal != null)
                {

                    string? encryptedEmail = principal.FindFirst("Email")!.Value;

                    bool isMemberExist = await _checkMiddlewareClaimsService.IsUserExistMiddlewareService(encryptedEmail);

                    if (isMemberExist)
                        return Ok("Token valid!");

                    return Unauthorized("Fail");
                }
                else
                {
                    return Unauthorized("Fail");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
