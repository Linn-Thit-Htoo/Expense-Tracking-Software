using ExpenseTrackerApi.Models.Entities;

namespace ExpenseTrackerApi.Services
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Register Service
        public async Task<int> RegisterService(UserDataModel userDataModel)
        {
            try
            {
                // default values
                userDataModel.UserRole = "user";
                userDataModel.CreateDate = DateTime.Now;
                userDataModel.IsActive = true;

                await _appDbContext.Users.AddAsync(userDataModel);
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
