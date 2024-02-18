using ExpenseTrackerApi.Models.Entities;

namespace ExpenseTrackerApi.Services
{
    public class BalanceService
    {
        private readonly AppDbContext _appDbContext;

        public BalanceService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Create balance service
        public async Task<int> CreateBalanceService(long userID)
        {
            try
            {
                if (userID == 0)
                    return 0;

                BalanceDataModel model = new()
                {
                    UserId = userID,
                    Amount = 0
                };

                await _appDbContext.Balance.AddAsync(model);
                int result = await _appDbContext.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
