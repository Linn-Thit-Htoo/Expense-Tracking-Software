using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApi.Models.RequestModels
{
    public class LoginRequestModel
    {
        [EmailAddress(ErrorMessage = "Email is invalid!")]
        public  required string Email { get; set; }
        public required string Password { get; set; }
    }
}
