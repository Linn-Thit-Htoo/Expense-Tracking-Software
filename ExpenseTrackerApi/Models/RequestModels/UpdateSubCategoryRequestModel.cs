namespace ExpenseTrackerApi.Models.RequestModels
{
    public class UpdateSubCategoryRequestModel
    {
        public required long SubCategoryId { get; set; }
        public required long CategoryId { get; set; }
        public required string SubCategoryName { get; set; }
    }
}
