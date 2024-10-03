using BusinessObject.Dto.RequestDto;
using BusinessObject.DTO.RequestDto;
using BusinessObject.DTO.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IPromotionService
    {
        Task<List<ViewDetailsPromotionDto>> GetAllPromotions();
        Task<ViewDetailsPromotionDto> GetPromotionById(int id);
        Task CreatePromotion(CreatePromotionDto createPromotionDto);
        Task UpdatePromotion(UpdatePromotionDto updatePromotionDto);
        Task DeletePromotion(int id);
        Task<ViewDetailsPromotionDto> ChangeAvailability(int id);
        Task<List<ViewDetailsPromotionDto>> SearchPromotions(string? name, decimal? price, bool? isAvailable, int? durationInDays);
    }
}