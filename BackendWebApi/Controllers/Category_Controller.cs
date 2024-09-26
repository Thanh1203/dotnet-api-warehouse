using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class Category_Controller(ICategory ICategory) : Controller
    {
        private readonly ICategory _ICategory = ICategory;
        private readonly int companyId = GlobalConstant.CompanyId;
        [HttpGet]
        public async Task<IActionResult> FetchCategories(string? name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Ok(await _ICategory.GetCategories(companyId));
                }
                else
                {
                    return Ok(await _ICategory.SearchCategory(name, companyId));
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory ([FromBody] Category category)
        {
            try
            {
                await _ICategory.Create(category, companyId);
                return Ok("Create successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] Category category)
        {
            try
            {
                await _ICategory.Update(category, companyId);
                return Ok("Update successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory([FromBody] List<int> idsDelete)
        {
            try
            {
                await _ICategory.Delete(idsDelete, companyId);
                return Ok("Delete successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
