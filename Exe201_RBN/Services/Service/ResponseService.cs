using AutoMapper;
using BusinessObject.Dto;
using BusinessObject;
using Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Repositories;
using System.ComponentModel.Design;
using BusinessObject.DTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Services.Service
{
    public class ResponseService : IResponseService
    {
        private readonly IResponseRepository _responseRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public ResponseService(IResponseRepository responseRepository, IMapper mapper, IFeedbackRepository feedbackRepository, ICompanyService companyService)
        {
            _responseRepository = responseRepository ?? throw new ArgumentNullException(nameof(responseRepository));
            _feedbackRepository = feedbackRepository ?? throw new ArgumentNullException(nameof(feedbackRepository));
            _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<List<ResponseDTO>> GetResponseByFeedbackId(int id)
        {
            var feedback = await _feedbackRepository.GetFeedbackById(id);
            if (feedback == null)
            {
                throw new Exception("Not found feedback");
            }
            else
            {
                var response = await _responseRepository.GetResponseByFeedbackId(feedback.Id);
                return _mapper.Map<List<ResponseDTO>>(response);
            }
        }
        public async Task<List<Response>> GetAllResponse()
        {
            return await _responseRepository.GetAllResponse();
        }

        public async Task Create(ResponseDTO responseDTO)
        {
            if (responseDTO == null)
            {
                throw new ArgumentNullException(nameof(responseDTO));
            }

            if (responseDTO.FeedBackId == 0 && responseDTO.CompanyId == 0)
            {
                throw new ArgumentException("Either FeedbackId or CompanyId must be provided.");
            }

            if (responseDTO.FeedBackId != 0)
            {
                var feedback = await _feedbackRepository.GetFeedbackById(responseDTO.FeedBackId);
                if (feedback == null)
                {
                    throw new KeyNotFoundException($"Feedback with id {responseDTO.FeedBackId} does not exist.");
                }
            }

            if (responseDTO.CompanyId != 0)
            {
                var company = await _companyService.GetCompanyById(responseDTO.CompanyId);
                if (company == null)
                {
                    throw new KeyNotFoundException($"Company with id {responseDTO.CompanyId} does not exist.");
                }
            }

            var createResponse = _mapper.Map<Response>(responseDTO);
            await _responseRepository.Create(createResponse);
        }

        public async Task Delete(int id)
        {
            await _responseRepository.Delete(id);
        }

        public async Task<Response> GetResponseById(int id)
        {
            return await _responseRepository.GetResponseById(id);
        }

        public async Task UpdateResponse(ResponseDTO responseDTO, int id)
        {
            var existingResponse = await _responseRepository.GetResponseById(id);
            if (existingResponse == null)
            {
                throw new KeyNotFoundException($"Response with id {id} not found.");
            }

            if (responseDTO.FeedBackId == 0 && responseDTO.CompanyId == 0)
            {
                throw new ArgumentException("Either FeedbackId or CompanyId must be provided.");
            }

            if (responseDTO.FeedBackId != 0)
            {
                var fb = await _feedbackRepository.GetFeedbackById(responseDTO.FeedBackId);
                if (fb == null)
                {
                    throw new KeyNotFoundException($"Feedback with id {responseDTO.FeedBackId} does not exist.");
                }
            }

            if (responseDTO.CompanyId != 0)
            {
                var company = await _companyService.GetCompanyById(responseDTO.CompanyId);
                if (company == null)
                {
                    throw new KeyNotFoundException($"Company with id {responseDTO.CompanyId} does not exist.");
                }
            }

            _mapper.Map(responseDTO, existingResponse);
            await _responseRepository.Update(existingResponse, id);
        }
    }
}