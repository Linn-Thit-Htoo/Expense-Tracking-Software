using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApi.Models.Entities
{
    public class BalanceDataModel
    {
        [Key]
        public long BalanceId { get; set; }
        public long UserId { get; set; }
        public double Amount { get; set; }
        public string? UpdateDate { get; set; }
    }
}
