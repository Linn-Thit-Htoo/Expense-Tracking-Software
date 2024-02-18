using ExpenseTrackerApi.Models.Entities;
using ExpenseTrackerApi.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public BalanceController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBalance([FromBody] UpdateBalanceRequestModel model)
        {
            try
            {
                UserDataModel? user = await _appDbContext.Users.Where(x => x.UserId == model.UserId && x.IsActive == true).FirstOrDefaultAsync();
                if (user is null)
                    return NotFound("User not found.");

                BalanceDataModel? item = await _appDbContext.Balance.Where(x => x.UserId == user.UserId).FirstOrDefaultAsync();
                if (item is null)
                    return NotFound("No data found.");

                item.Amount = model.Amount;
                item.UpdateDate = model.UpdateDate;

                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status202Accepted, "Balance Updated.") : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
