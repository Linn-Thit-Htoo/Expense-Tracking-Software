using ExpenseTrackerApi.Models.Entities;
using ExpenseTrackerApi.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public SubCategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region get sub categories
        [HttpGet]
        public async Task<IActionResult> GetSubCategories(string? isActive, int categoryID)
        {
            try
            {
                List<SubCategoryDataModel> lst = new();

                if (categoryID == 0 && string.IsNullOrEmpty(isActive))
                    goto Fail;

                // category id without is active
                if (string.IsNullOrEmpty(isActive) && categoryID != 0)
                {
                    lst = await _appDbContext.SubCategories
                        .Where(x => x.CategoryId == categoryID)
                        .OrderByDescending(x => x.SubCategoryId)
                        .AsNoTracking()
                        .ToListAsync();
                }
                // both category id and is active
                else if (!string.IsNullOrEmpty(isActive) && categoryID != 0)
                {
                    if (isActive == "0")
                    {
                        lst = await _appDbContext.SubCategories
                            .Where(x => x.IsActive == false && x.CategoryId == categoryID)
                            .OrderByDescending(x => x.SubCategoryId)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else if (isActive == "1")
                    {
                        lst = await _appDbContext.SubCategories
                            .Where(x => x.IsActive == true && x.CategoryId == categoryID)
                            .OrderByDescending(x => x.SubCategoryId)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        goto Fail;
                    }
                }
                // is active without categoryID
                else if(!string.IsNullOrEmpty(isActive) && categoryID == 0)
                {
                    if (isActive == "0")
                    {
                        lst = await _appDbContext.SubCategories
                            .Where(x => x.IsActive == false)
                            .OrderByDescending(x => x.SubCategoryId)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else if (isActive == "1")
                    {
                        lst = await _appDbContext.SubCategories
                            .Where(x => x.IsActive == true)
                            .OrderByDescending(x => x.SubCategoryId)
                            .AsNoTracking()
                            .ToListAsync();
                    }
                    else
                    {
                        goto Fail;
                    }
                }
                return Ok(lst);

            Fail:
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region create sub category
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubCategoryDataModel model)
        {
            try
            {
                if (model.CategoryId == 0 ||
                    string.IsNullOrEmpty(model.SubCategoryName))
                    return BadRequest();

                bool isDuplicate = await _appDbContext.SubCategories
                    .AnyAsync(x => x.SubCategoryName == model.SubCategoryName && x.IsActive == true);

                if (isDuplicate)
                    return Conflict("Sub Category name already exists!");

                model.IsActive = true;
                await _appDbContext.SubCategories.AddAsync(model);
                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status201Created, "Sub Category created.") : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update sub category
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSubCategoryRequestModel model)
        {
            try
            {
                SubCategoryDataModel? item = await _appDbContext.SubCategories
                    .Where(x => x.SubCategoryId == model.SubCategoryId && x.IsActive == true)
                    .FirstOrDefaultAsync();
                if (item is null)
                    return NotFound("No data found.");

                bool isDuplicate = await _appDbContext.SubCategories
                    .Where(x => x.SubCategoryName == model.SubCategoryName && x.SubCategoryId != model.SubCategoryId)
                    .AnyAsync();
                if (isDuplicate)
                    return Conflict("Sub Category name already exists!");

                item.CategoryId = model.SubCategoryId;
                item.SubCategoryName = model.SubCategoryName;
                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status202Accepted, "Sub Category updated.") : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete sub category
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                SubCategoryDataModel? item = await _appDbContext.SubCategories
                    .Where(x => x.SubCategoryId == id && x.IsActive == true)
                    .FirstOrDefaultAsync();
                if (item is null)
                    return NotFound("No data found.");

                item.IsActive = false;
                int result = await _appDbContext.SaveChangesAsync();

                return result > 0 ? StatusCode(StatusCodes.Status202Accepted, "Sub Category deleted.") : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
