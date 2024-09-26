using BackendWebApi.Helpers;
using BackendWebApi.Interfaces;
using BackendWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendWebApi.Controllers
{
    [Route("api/producer")]
    [ApiController]
    public class Producer_Controller(IProducer IProducer) : Controller
    {
        private readonly IProducer _IProducer = IProducer;
        private readonly int companyId = GlobalConstant.CompanyId;

        [HttpGet]
        public async Task<IActionResult> FetchProducer(string? name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Ok(await _IProducer.GetProducers(companyId));
                }
                else
                {
                    return Ok(await _IProducer.SearchProducer(name, companyId));
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducer([FromBody] Producer producer)
        {
            try
            {
                await _IProducer.Create(producer, companyId);
                return Ok("Create successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProducer([FromBody] Producer producer)
        {
            try
            {
                await _IProducer.Update(producer, companyId);
                return Ok("Update successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProducer([FromBody] List<int> idsDelete)
        {
            try
            {
                await _IProducer.Delete(idsDelete, companyId);
                return Ok("Delete successful!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
