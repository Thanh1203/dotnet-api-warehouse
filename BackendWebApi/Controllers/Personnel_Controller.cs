using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Controllers
{
    [Route("api/personnel")]
    [ApiController]
    public class Personnel_Controller(IPersonnel IPersonnel) : Controller
    {
        private readonly IPersonnel _IPersonnel = IPersonnel;
        private readonly int companyId = GlobalConstant.CompanyId;

        [HttpGet]
        public async Task<IActionResult> FetchPersonnels(string? code, string? name, string? address) 
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(address))
                {
                    return Ok(await _IPersonnel.GetPersonnels(companyId)); 
                }
                else
                {
                    return Ok(await _IPersonnel.SearchPersonnel(code, name, address, companyId));
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePersonnel([FromBody] Personnel personnel)
        {
            try
            {
                await _IPersonnel.Create(personnel, companyId);
                return Ok("Create successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonnel([FromBody] Personnel personnel)
        {
            try
            {
                await _IPersonnel.Update(personnel, companyId);
                return Ok("Update successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePersonnel([FromBody] List<int> idsDelete)
        {
            try
            {
                await _IPersonnel.Delete(idsDelete, companyId);
                return Ok("Delete successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("address")]
        public async Task<IActionResult> GetAddressPersonnel()
        {
            try
            {
                var address = await _IPersonnel.GetAddressPersonnel(companyId);
                return Ok(address);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }
    }
}
