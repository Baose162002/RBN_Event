using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using System.Globalization;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.Dto;

namespace Services.Configurations.Mapper
{
    public class MappingEntities : Profile
    {
        public MappingEntities()
        {
            CreateMap<CompanyDTO, Company>();
            CreateMap<EventDTO, Event>()
            .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.ParseExact(src.CreateAt, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.ParseExact(src.UpdateAt, "dd/MM/yyyy", CultureInfo.InvariantCulture)));

            CreateMap<User, UserResponseDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<FeedbackDTO, FeedBack>();
            CreateMap<ResponseDTO, Response>();

        }
    }
}
