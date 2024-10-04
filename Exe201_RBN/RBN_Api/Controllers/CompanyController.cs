using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace RBN_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCompany()
        {
            var companies = await _companyService.GetAllCompany();
            if (companies == null || !companies.Any())
            {
                return NotFound("No company found");
            }
            return Ok(companies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody]CompanyDTO company)
        {
            await _companyService.Create(company);
            return Ok("Add company successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletCompany(int id)
        {
            await _companyService.Delete(id);
            return Ok("Delete successfully");
        }

        [HttpPut("companyid")]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyDTO company, int companyid)
        {
            try
            {
                await _companyService.UpdateCompany(company, companyid);
                return Ok("Update successfully");
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            var companies = await _companyService.GetCompanyById(id);
            if (companies == null)
            {
                return NotFound("No company found");
            }
            return Ok(companies);
        }
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetCompanyByIdUser(int id)
        {
            var companies = await _companyService.GetCompanyByIdUser(id);
            if (companies == null)
            {
                return NotFound("No company found");
            }
            return Ok(companies);
        }
    }

    
}
