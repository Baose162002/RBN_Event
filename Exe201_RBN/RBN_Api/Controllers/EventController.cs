using BusinessObject.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using Services.Service;

namespace RBN_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
		private readonly IEventImgService _eventImgService;
		public EventController(IEventService eventService, IEventImgService eventImgService)
        {
            _eventService = eventService;
			_eventImgService = eventImgService;
		}
        [HttpGet]
        public async Task<IActionResult> GetAllEvent()
        {
            var events = await _eventService.GetAllEvent();
            if (events == null || !events.Any())
            {
                return NotFound("Event not found");
            }
            return Ok(events);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventDTO eventDTO)
        {
            try
            {
                await _eventService.Create(eventDTO);
                return Ok("Add event successfully");
            }catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("id")]
        public async Task<IActionResult> UpdateEvent(EventDTO eventDTO, int id)
        {
            try
            {
                await _eventService.Update(eventDTO, id);
                return Ok("Update event successfully");
            }catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _eventService.Delete(id);
                return Ok("Delete event successfully");
            }catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetEventById(int id)
        {
            
             var events = await _eventService.GetEventById(id);
             if(events == null)
             {
                return NotFound("Event not found");
             }
                return Ok(events);
       
        }

		[HttpPost("Upload")]
		public async Task<IActionResult> UploadEventImage(IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				return BadRequest("Invalid file.");
			}

			try
			{
				await _eventImgService.UploadFile(file);
				return Ok("Image uploaded successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

	}
}
