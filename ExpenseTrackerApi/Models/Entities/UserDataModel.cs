using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerApi.Models.Entities
{
    public class UserDataModel
    {
        [Key]
        public long UserId { get; set; }
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email is invalid!")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string? UserRole { get; set; }
        public string CreateDate { get; set; }
        public bool IsActive { get; set; }
    }
}
