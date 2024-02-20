using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrackerApi.Models.Entities
{
    [Table("SubCategories")]
    public class SubCategoryDataModel
    {
        [Key]
        public long SubCategoryId { get; set; }
        public long CategoryId { get; set; }
        public string SubCategoryName { get; set;}
        public bool IsActive { get; set; }
    }
}
