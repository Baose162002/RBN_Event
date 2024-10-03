using AutoMapper;
using BusinessObject;
using Repositories.Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class PromotionService : IPromotionService
    {
        private readonly IBaseRepository<Promotion> _promotionRepo;
        private readonly IMapper mapper;
        public PromotionService(IBaseRepository<Promotion> promotion, IMapper mapper)
        {
            _promotionRepo ??= promotion;
            mapper ??= mapper;
        }
        public async Task<List<Promotion>> GetAllPromotion()
        {
            return await _promotionRepo.GetAllAsync();
        }

    }
}
