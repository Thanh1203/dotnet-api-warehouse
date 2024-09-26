using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class Customer_Controller(ICustomer customer) : Controller
    {
        private readonly ICustomer _ICustomer = customer;
        private readonly int companyId = GlobalConstant.CompanyId;

        [HttpGet]
        public async Task<IActionResult> FetchCustomer(string? phoneNumber)
        {
            try
            {
                return Ok(await _ICustomer.SearchCustomer(phoneNumber, companyId));
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
