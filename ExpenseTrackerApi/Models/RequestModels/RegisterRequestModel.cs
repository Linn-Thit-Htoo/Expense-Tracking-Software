using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApi.Models.RequestModels
{
    public class RegisterRequestModel
    {
        [EmailAddress(ErrorMessage = "Email is invalid!")]
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string CreateDate { get; set; }
    }
}
