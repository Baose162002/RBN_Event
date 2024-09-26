using BusinessObject.Dto;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.Service;

namespace RBN_Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFeedback()
        {
            var feedbacks = await _feedbackService.GetAllFeedback();
            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound("No feedback found");
            }
            return Ok(feedbacks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeedback(FeedbackDTO feedbackDTO)
        {
            try
            {
                await _feedbackService.Create(feedbackDTO);
                return Ok("Add feedback successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _feedbackService.Delete(id);
                return Ok("Delete feedback successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetEventById(int id)
        {

            var events = await _feedbackService.GetFeedbackById(id);
            if (events == null)
            {
                return NotFound("Feedback not found");
            }
            return Ok(events);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, FeedbackDTO feedbackDTO)
        {
            try
            {
                await _feedbackService.UpdateFeedback(feedbackDTO, id);
                return Ok("Feedback updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the feedback: {ex.Message}");
            }
        }
    }
}
