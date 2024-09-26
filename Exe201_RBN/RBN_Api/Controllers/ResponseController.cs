using BusinessObject.Dto;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace RBN_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponseController : Controller
    {
        private readonly IResponseService _responseService;

        public ResponseController(IResponseService responseService)
        {
            _responseService = responseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllResponses()
        {
            var responses = await _responseService.GetAllResponse();
            if (responses == null || !responses.Any())
            {
                return NotFound("No responses found");
            }
            return Ok(responses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateResponse(ResponseDTO responseDTO)
        {
            try
            {
                await _responseService.Create(responseDTO);
                return Ok("Add response successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _responseService.Delete(id);
                return Ok("Delete response successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetResponseById(int id)
        {
            var response = await _responseService.GetResponseById(id);
            if (response == null)
            {
                return NotFound("Response not found");
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResponse(int id, ResponseDTO responseDTO)
        {
            try
            {
                await _responseService.UpdateResponse(responseDTO, id);
                return Ok("Response updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the response: {ex.Message}");
            }
        }
    }
}