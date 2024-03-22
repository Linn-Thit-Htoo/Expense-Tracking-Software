using ExpenseTrackerApi.Models.Entities;
using ExpenseTrackerApi.Models.RequestModels;
using ExpenseTrackerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ExpenseTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserService _userService;
        private readonly GenerateTokenService _generateTokenService;

        public UserController(AppDbContext appDbContext, UserService userService, GenerateTokenService generateTokenService)
        {
            _appDbContext = appDbContext;
            _userService = userService;
            _generateTokenService = generateTokenService;
        }

        #region testing
        [HttpGet]
        [Route("testing")]
        public IActionResult Testing()
        {
            return Ok("nice");
        }
        #endregion

        #region Register
        [HttpPost("account/register")]
        public async Task<IActionResult> Register([FromBody] UserDataModel userDataModel)
        {
            try
            {
                if (userDataModel is null || string.IsNullOrWhiteSpace(userDataModel.UserName) || string.IsNullOrEmpty(userDataModel.Email) || string.IsNullOrEmpty(userDataModel.Password) || string.IsNullOrEmpty(userDataModel.CreateDate))
                {
                    return BadRequest();
                }

                // Email is unique
                UserDataModel? item = await _appDbContext.Users.Where(x => x.Email == userDataModel.Email).FirstOrDefaultAsync();

                if (item is not null)
                    return Conflict("User with this Email already exists. Please login.");

                int result = await _userService.RegisterService(userDataModel);

                return result > 0 ? StatusCode(StatusCodes.Status201Created, "Registration Successful!") : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Login
        [HttpPost("account/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            try
            {
                UserDataModel? item = await _appDbContext.Users.Where(x => x.Email == model.Email && x.Password == model.Password && x.IsActive == true).FirstOrDefaultAsync();
                if (item is null)
                    goto LoginFail;

                BalanceDataModel? balance = await _appDbContext.Balance.Where(x => x.UserId == item.UserId).FirstOrDefaultAsync();
                if (balance is null)
                    goto LoginFail;

                if (item is not null && balance is not null)
                {
                    List<Claim> claims = _userService.GetUserClaimsService(item, balance);
                    return Ok(new JwtSecurityTokenHandler().WriteToken(_generateTokenService.GenerateToken(claims)));
                }

            LoginFail:
                return Unauthorized("Login Fail!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
