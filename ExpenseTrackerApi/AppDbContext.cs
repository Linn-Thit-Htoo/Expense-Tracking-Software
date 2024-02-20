using ExpenseTrackerApi.Models;
using ExpenseTrackerApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserDataModel> Users { get; set; }
        public DbSet<BalanceDataModel> Balance { get; set; }
        public DbSet<CategoryDataModel> Categories { get; set; }
        public DbSet<SubCategoryDataModel> SubCategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        //public DbSet<Category> Categories { get; set; }
    }
}
