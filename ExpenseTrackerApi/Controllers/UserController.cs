using ExpenseTrackerApi.Models.Entities;
using ExpenseTrackerApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserService _userService;

        public UserController(AppDbContext appDbContext, UserService userService)
        {
            _appDbContext = appDbContext;
            _userService = userService;
        }

        #region Register
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDataModel userDataModel)
        {
            try
            {
                if (userDataModel is null || string.IsNullOrWhiteSpace(userDataModel.UserName) || string.IsNullOrEmpty(userDataModel.Email) || string.IsNullOrEmpty(userDataModel.Password))
                {
                    return BadRequest();
                }

                // Email is unique
                UserDataModel? item = await _appDbContext.Users.Where(x => x.Email == userDataModel.Email).FirstOrDefaultAsync();

                if (item is not null) return Conflict("User with this Email already exists. Please login.");

                int result = await _userService.RegisterService(userDataModel);

                return result > 0 ? StatusCode(StatusCodes.Status201Created, "Registration Successful!") : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
