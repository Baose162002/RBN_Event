using AutoMapper;
using BusinessObject.DTO;
using BusinessObject;
using Repositories.IRepositories;
using Repositories.Repositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusinessObject.Dto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Services.Service
{
    public class FeedbackService:IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IMapper mapper, IUserService userService, ICompanyService companyService)
        {
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _userService = userService;
            _companyService = companyService;
        }
        public async Task<List<FeedbackDTO>> GetFeedBacksByCompanyIdAsync(int companyId)
        {
            var company = await _companyService.GetCompanyById(companyId);
            if (company == null)
            {
                throw new Exception("Not found company");
            }
            else
            {
                var bookings = await _feedbackRepository.GetFeedBacksByCompanyIdAsync(company.Id);
                return _mapper.Map<List<FeedbackDTO>>(bookings);
            }
        }
        public async Task<List<FeedBack>> GetAllFeedback()
        {
            return await _feedbackRepository.GetAllFeedback();
        }
        public async Task Create(FeedbackDTO feedback)
        {
            var updatefeedback = _mapper.Map<FeedBack>(feedback);
            await _feedbackRepository.Create(updatefeedback);
        }
    

        public async Task Delete(int id)
        {
            await _feedbackRepository.Delete(id);
        }
        public async Task<FeedBack> GetFeedbackById(int id)
        {
            return await _feedbackRepository.GetFeedbackById(id);
        }

        public async Task UpdateFeedback(FeedbackDTO feedbackDto, int id)
        {
            // Get the existing feedback
            var existingFeedback = await _feedbackRepository.GetFeedbackById(id);

            if (existingFeedback == null)
            {
                throw new KeyNotFoundException($"Feedback with id {id} not found.");
            }
            if (existingFeedback.UserId == 0 && existingFeedback.CompanyId == 0)
            {
                throw new ArgumentException("Either UserId or CompanyId must be provided.");
            }
           
                    var fb = await _userService.GetUserByIdAsync(feedbackDto.UserId);
                    if (fb == null)
                    {
                        throw new KeyNotFoundException($"User with id {feedbackDto.UserId} does not exist.");
                    }

                var company = await _companyService.GetCompanyById(feedbackDto.CompanyId);
                if (company == null)
                {
                    throw new KeyNotFoundException($"Company with id {feedbackDto.CompanyId} does not exist.");
                }


            // Map the DTO to the existing entity
            _mapper.Map(feedbackDto, existingFeedback);

            // Update the feedback
            await _feedbackRepository.Update(existingFeedback, id);
        }




    }
}
