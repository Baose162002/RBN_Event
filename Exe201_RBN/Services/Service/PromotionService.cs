using AutoMapper;
using BusinessObject;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO.RequestDto;
using BusinessObject.DTO.ResponseDto;
using Repositories.Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Service
{
    public class PromotionService : IPromotionService
    {
        private readonly IBaseRepository<Promotion> _promotionRepo;
        private readonly IMapper _mapper;

        public PromotionService(IBaseRepository<Promotion> promotionRepo, IMapper mapper)
        {
            _promotionRepo = promotionRepo;
            _mapper = mapper;
        }

        public async Task<List<ViewDetailsPromotionDto>> GetAllPromotions()
        {
            var promotions = await _promotionRepo.GetAllAsync();
            return _mapper.Map<List<ViewDetailsPromotionDto>>(promotions);
        }

        public async Task<ViewDetailsPromotionDto> GetPromotionById(int id)
        {
            var promotion = await _promotionRepo.GetByIdAsync(id);
            if (promotion == null)
            {
                throw new KeyNotFoundException($"Promotion with ID {id} not found.");
            }
            return _mapper.Map<ViewDetailsPromotionDto>(promotion);
        }

        public async Task CreatePromotion(CreatePromotionDto createPromotionDto)
        {
            var promotion = _mapper.Map<Promotion>(createPromotionDto);
            await _promotionRepo.CreateAsync(promotion);
        }

        public async Task UpdatePromotion(UpdatePromotionDto updatePromotionDto)
        {
            var existingPromotion = await _promotionRepo.GetByIdAsync(updatePromotionDto.Id);
            if (existingPromotion == null)
            {
                throw new KeyNotFoundException($"Promotion with ID not found.");
            }

            _mapper.Map(updatePromotionDto, existingPromotion);
            await _promotionRepo.UpdateAsync(existingPromotion);
        }

        public async Task DeletePromotion(int id)
        {
            var promotion = await _promotionRepo.GetByIdAsync(id);
            if (promotion == null)
            {
                throw new KeyNotFoundException($"Promotion with ID {id} not found.");
            }

            await _promotionRepo.RemoveAsync(promotion);
        }

        public async Task<ViewDetailsPromotionDto> ChangeAvailability(int id)
        {
            var promotion = await _promotionRepo.GetByIdAsync(id);
            if (promotion == null)
            {
                throw new KeyNotFoundException($"Promotion with ID {id} not found.");
            }

            promotion.IsAvailable = !promotion.IsAvailable;
            await _promotionRepo.UpdateAsync(promotion);

            return _mapper.Map<ViewDetailsPromotionDto>(promotion);
        }

        public async Task<List<ViewDetailsPromotionDto>> SearchPromotions(string? name, decimal? price, bool? isAvailable, int? durationInDays)
        {
            var query = await _promotionRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
            }

            if (price.HasValue)
            {
                query = query.Where(p => p.Price == price.Value).ToList();
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(p => p.IsAvailable == isAvailable.Value).ToList();
            }

            if (durationInDays.HasValue)
            {
                query = query.Where(p => p.DurationInDays == durationInDays.Value).ToList();
            }

            var promotions = query.ToList();
            return _mapper.Map<List<ViewDetailsPromotionDto>>(promotions);
        }
    }
}
