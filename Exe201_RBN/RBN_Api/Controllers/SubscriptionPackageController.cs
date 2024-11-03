using BusinessObject.DTO;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace RBN_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPackageController : ControllerBase
    {
        private readonly ISubscriptionPackageService _subscriptionPackageService;

        public SubscriptionPackageController(ISubscriptionPackageService subscriptionPackageService)
        {
            _subscriptionPackageService = subscriptionPackageService;
        }
        [HttpPut("register-subsciptionpackage")]
        public async Task<IActionResult> CompanyRegisterSubscriptionPackage(string email, int subscriptionPackageId)
        {

            await _subscriptionPackageService.CompanyRegisterSubscriptionPackage(email, subscriptionPackageId);
            return Ok("Mua gói thành công");
        }

        [HttpGet]
        public async Task<ActionResult<List<ListSubscriptionPackageDTO>>> GetAllSubscriptionPackages()
        {
            var packages = await _subscriptionPackageService.GetAllSubscriptionPackages();
            return Ok(packages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListSubscriptionPackageDTO>> GetSubscriptionPackageById(int id)
        {
            var package = await _subscriptionPackageService.GetSubscriptionPackageById(id);
            if (package == null)
            {
                return NotFound();
            }
            return Ok(package);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSubscriptionPackage([FromBody] SubscriptionPackageDTO package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _subscriptionPackageService.CreateSubscriptionPackage(package);
            return CreatedAtAction(nameof(GetSubscriptionPackageById), new { id = package.Name }, package);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscriptionPackage(int id, [FromBody] SubscriptionPackageDTO package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _subscriptionPackageService.UpdateSubscriptionPackage(package, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriptionPackage(int id)
        {
            await _subscriptionPackageService.DeleteSubscriptionPackage(id);
            return NoContent();
        }
    }
}
