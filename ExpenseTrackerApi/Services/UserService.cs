using ExpenseTrackerApi.Models.Entities;
using System.Security.Claims;

namespace ExpenseTrackerApi.Services
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;
        private readonly EncryptService _encryptService;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext appDbContext, EncryptService encryptService, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _encryptService = encryptService;
            _configuration = configuration;
        }

        #region Register Service
        public async Task<int> RegisterService(UserDataModel userDataModel)
        {
            try
            {
                // default values
                userDataModel.UserRole = "user";
                userDataModel.CreateDate = userDataModel.CreateDate;
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

        #region Get user claims service
        public List<Claim> GetUserClaimsService(UserDataModel user)
        {
            try
            {
                List<Claim> claims = new()
                {
                   new Claim("UserId", _encryptService.EncryptString(user.UserId.ToString(), _configuration["EncryptionKey"]!), ClaimValueTypes.Integer),
                   new Claim("UserName", _encryptService.EncryptString(user.UserName, _configuration["EncryptionKey"]!)),
                   new Claim("Email", _encryptService.EncryptString(user.Email, _configuration["EncryptionKey"]!)),
                };

                return claims;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        #endregion
    }
}
