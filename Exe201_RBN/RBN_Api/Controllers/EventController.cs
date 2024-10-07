using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO;
using BusinessObject.DTO.RequestDto;
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
        [HttpGet("get-event-by-company/{id}")]
        public async Task<List<EventDTO>> GetBookingsByCompanyIdAsync(int id)
        {
            return await _eventService.GetEventsByCompanyIdAsync(id);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEvent(string? searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _eventService.GetAllEvent(searchTerm, pageNumber, pageSize);
            if (result == null || !result.Data.Any())
            {
                return NotFound("No events found.");
            }

            return Ok(result);
        }

        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetAllEventsByCompanyId(int id, string? searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _eventService.GetAllEventsByCompanyId(id, searchTerm, pageNumber, pageSize);
            if (result == null || !result.Data.Any())
            {
                return NotFound("No events found.");
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventDto eventDTO)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(UpdateEventDto eventDTO, int id)
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
        [HttpDelete("{id}")]
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

        [HttpGet("{id}")]
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
                // Upload file và lấy ID của ảnh đã upload
                var imageId = await _eventImgService.UploadFile(file); // Giả sử phương thức này trả về ID của ảnh

                // Tạo một đối tượng kết quả để trả về
                var result = new UploadImageResponse
                {
                    Status = "success",
                    Message = "Image uploaded successfully.",
                    ImageId = imageId // Trả về ID của ảnh
                };

                return Ok(result); // Trả về đối tượng JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
