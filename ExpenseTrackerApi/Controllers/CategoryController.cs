using ExpenseTrackerApi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Get categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                List<CategoryDataModel> lst = await _appDbContext.Categories.OrderByDescending(x => x.CategoryId).AsNoTracking().ToListAsync();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                CategoryDataModel? item = await _appDbContext.Categories
                    .Where(x => x.CategoryId == id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                return item is null ? NotFound("No data found.") : Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region create category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDataModel model)
        {
            try
            {
                CategoryDataModel? item = await _appDbContext.Categories
                    .Where(x => x.CategoryName == model.CategoryName)
                    .FirstOrDefaultAsync();
                if (item is not null)
                    return Conflict("Category Name already exists!");

                await _appDbContext.Categories.AddAsync(model);
                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status201Created, "Category Created.") : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region update category
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDataModel model)
        {
            try
            {
                CategoryDataModel? item = await _appDbContext.Categories
                    .Where(x => x.CategoryId == model.CategoryId)
                    .FirstOrDefaultAsync();
                if (item is null)
                    return NotFound("No data found.");

                bool isDuplicate = await _appDbContext.Categories
                    .Where(x => x.CategoryName == model.CategoryName && x.CategoryId != model.CategoryId)
                    .AnyAsync();
                if (isDuplicate)
                    return Conflict("Category Name already exists!");

                item.CategoryName = model.CategoryName;
                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status202Accepted, "Category Updated.") : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
