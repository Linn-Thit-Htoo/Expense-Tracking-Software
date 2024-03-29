﻿using ExpenseTrackerApi.Models.Entities;
using System.Security.Claims;

namespace ExpenseTrackerApi.Services
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;
        private readonly EncryptService _encryptService;
        private readonly IConfiguration _configuration;
        private readonly BalanceService _balanceService;
        public UserService(AppDbContext appDbContext, EncryptService encryptService, IConfiguration configuration, BalanceService balanceService)
        {
            _appDbContext = appDbContext;
            _encryptService = encryptService;
            _configuration = configuration;
            _balanceService = balanceService;
        }

        #region Register Service
        public async Task<int> RegisterService(UserDataModel userDataModel)
        {
            using var transaction = _appDbContext.Database.BeginTransaction();

            try
            {
                userDataModel.UserRole = "user";
                userDataModel.CreateDate = userDataModel.CreateDate;
                userDataModel.IsActive = true;

                await _appDbContext.Users.AddAsync(userDataModel);
                int result = await _appDbContext.SaveChangesAsync();

                int balanceRowEffected = await _balanceService.CreateBalanceService(userDataModel.UserId);

                if (result > 0 && balanceRowEffected > 0)
                {
                    transaction.Commit();
                    return 1;
                }

                else
                {
                    transaction.Rollback();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
        #endregion


        #region Get user claims service
        public List<Claim> GetUserClaimsService(UserDataModel user, BalanceDataModel balance)
        {
            try
            {
                List<Claim> claims = new()
                {
                   new Claim("UserId", _encryptService.EncryptString(user.UserId.ToString(), _configuration["EncryptionKey"]!), ClaimValueTypes.Integer),
                   new Claim("UserName", _encryptService.EncryptString(user.UserName, _configuration["EncryptionKey"]!)),
                   new Claim("Email", _encryptService.EncryptString(user.Email, _configuration["EncryptionKey"]!)),
                   new Claim("Balance", _encryptService.EncryptString(Convert.ToString(balance.Amount), _configuration["EncryptionKey"]!))
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
