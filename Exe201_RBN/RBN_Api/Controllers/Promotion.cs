using BusinessObject;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace RBN_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _proService;

        public PromotionController(IPromotionService proService)
        {
            _proService = proService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ViewDetailsBookingDto>>> GetAllPromotion()
        {
            var promotions = await _proService.GetAllPromotion();
            return Ok(promotions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ViewDetailsBookingDto>> GetPromotionById(int id)
        {
            var promotions = await _proService.GetPromotionById(id);
            if (promotions == null)
            {
                return NotFound();
            }
            return Ok(promotions);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromBody] Promotion createPromotion)
        {
            await _proService.CreatePromotion(createPromotion);
            return Ok(new { message = "Tạo promotion thành công" });
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePromotion([FromBody] Promotion updatePromotion)
        {
            if (updatePromotion == null)
            {
                return BadRequest("Thông tin promotion không hợp lệ.");
            }

            await _proService.UpdatePromotion(updatePromotion);
            return Ok(new { message = "Cập nhật promotion thành công" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion(int id)
        {

            await _proService.DeletePromotion(id);
            return Ok(new { message = "Xóa promotion thành công" });
        }
        [HttpPut("change-status-prmotion/{id}")]
        public async Task<Promotion> ChangeStatus(int id)
        {
            var promotion = await _proService.ChangeAvailability(id);
            return promotion;
        }

        [HttpGet("search")]
        public async Task<List<Promotion>> SearchPromotions(string? name, decimal? price, bool? isAvailable, int? durationInDays)
        {
            return await _proService.SearchPromotions(name, price, isAvailable, durationInDays);
        }
    }
}
