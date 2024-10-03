using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO.RequestDto;
using BusinessObject.DTO.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }


        [HttpGet]
        public async Task<ActionResult<List<ViewDetailsPromotionDto>>> GetAllPromotions()
        {
            var promotions = await _promotionService.GetAllPromotions();
            return Ok(promotions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ViewDetailsPromotionDto>> GetPromotionById(int id)
        {
            try
            {
                var promotion = await _promotionService.GetPromotionById(id);
                return Ok(promotion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionDto createPromotionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _promotionService.CreatePromotion(createPromotionDto);
            // Lấy ID của Promotion vừa tạo để trả về trong CreatedAtAction
            var promotion = await _promotionService.SearchPromotions(createPromotionDto.Name, createPromotionDto.Price, createPromotionDto.IsAvailable, createPromotionDto.DurationInDays);
            if (promotion.Count > 0)
            {
                return CreatedAtAction(nameof(GetPromotionById), new { id = promotion[0].Id }, promotion[0]);
            }

            return CreatedAtAction(nameof(GetPromotionById), new { id = 0 }, createPromotionDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePromotion(int id, [FromBody] UpdatePromotionDto updatePromotionDto)
        {
            if (id != updatePromotionDto.Id)
            {
                return BadRequest("ID mismatch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _promotionService.UpdatePromotion(updatePromotionDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            try
            {
                await _promotionService.DeletePromotion(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpPatch("{id}/change-availability")]
        public async Task<ActionResult<ViewDetailsPromotionDto>> ChangeAvailability(int id)
        {
            try
            {
                var updatedPromotion = await _promotionService.ChangeAvailability(id);
                return Ok(updatedPromotion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpGet("search")]
        public async Task<ActionResult<List<ViewDetailsPromotionDto>>> SearchPromotions(
            [FromQuery] string? name,
            [FromQuery] decimal? price,
            [FromQuery] bool? isAvailable,
            [FromQuery] int? durationInDays)
        {
            var promotions = await _promotionService.SearchPromotions(name, price, isAvailable, durationInDays);
            return Ok(promotions);
        }
    }
}
