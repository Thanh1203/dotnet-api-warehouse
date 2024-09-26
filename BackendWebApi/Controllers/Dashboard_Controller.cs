using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class Dashboard_Controller(IDashboard dashboard) : Controller
    {
        private readonly IDashboard _IDashboard = dashboard;
        private readonly int companyId = GlobalConstant.CompanyId;

        [HttpGet]
        public async Task<IActionResult> FetchDataDashboard()
        {
            try
            {
                var data = await _IDashboard.GetInfoDashboard(companyId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
