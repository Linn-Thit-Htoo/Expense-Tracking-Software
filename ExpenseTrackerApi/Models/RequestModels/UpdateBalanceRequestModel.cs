using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApi.Models.RequestModels
{
    public class UpdateBalanceRequestModel
    {
        [Required]
        public long UserId { get; set; }
        public required double Amount { get; set; }
        public required string UpdateDate { get; set; }
    }
}
