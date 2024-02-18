using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApi.Models.Entities
{
    public class CategoryDataModel
    {
        [Key]
        public long CategoryId { get; set; }
        public required string CategoryName { get; set; }
    }
}
