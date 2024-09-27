using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using System.Globalization;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.Dto;
using BusinessObject.DTO.RequestDto;

namespace Services.Configurations.Mapper
{
    public class MappingEntities : Profile
    {
        public MappingEntities()
        {
            CreateMap<CompanyDTO, Company>();
			CreateMap<Company, CompanyDTO>();
			CreateMap<EventDTO, Event>()
            .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.ParseExact(src.CreateAt, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
            .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.ParseExact(src.UpdateAt, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
            CreateMap<CreateEventDto, Event>()
             .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.ParseExact(src.CreateAt, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
             .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.ParseExact(src.UpdateAt, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
            CreateMap<UpdateEventDto, Event>()
             .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.ParseExact(src.CreateAt, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
             .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.ParseExact(src.UpdateAt, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
            CreateMap<Event, EventDTO>()
		   .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
		   .ForMember(dest => dest.EventImg, opt => opt.MapFrom(src => src.EventImg));
			

			CreateMap<EventImg, EventImgDTO>();



			CreateMap<User, UserResponseDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<LoginUserRequest, User>();
            CreateMap<FeedbackDTO, FeedBack>();
            CreateMap<ResponseDTO, Response>();

            CreateMap<Booking, ViewDetailsBookingDto>();
        }
    }
}
