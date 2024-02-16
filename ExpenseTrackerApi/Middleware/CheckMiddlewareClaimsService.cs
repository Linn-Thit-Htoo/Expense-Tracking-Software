using ExpenseTrackerApi.Services;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Middleware
{
    public class CheckMiddlewareClaimsService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly DecryptService _decryptService;

        public CheckMiddlewareClaimsService(IConfiguration configuration, IServiceProvider serviceProvider, DecryptService decryptService)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _decryptService = decryptService;
        }

        public async Task<bool> IsUserExistMiddlewareService(string encryptedEmail)
        {
            string decryptedEmail = _decryptService.DecryptString(encryptedEmail, _configuration["EncryptionKey"]!);

            using var scope = _serviceProvider.CreateScope();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var user = await appDbContext.Users.Where(x => x.Email == decryptedEmail && x.IsActive == true).FirstOrDefaultAsync();

            return user is not null;
        }
    }
}
