using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using System.Globalization;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.Dto;
using BusinessObject.DTO.RequestDto;
using BusinessObject.DTO.ResponseDto;

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
             .ForMember(dest => dest.EventImgId, opt => opt.MapFrom(src => src.EventImgId))
             .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => DateTime.ParseExact(src.CreateAt, "dd/MM/yyyy", CultureInfo.InvariantCulture)))
             .ForMember(dest => dest.UpdateAt, opt => opt.MapFrom(src => DateTime.ParseExact(src.UpdateAt, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
            CreateMap<Event, EventDTO>()
           .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
           .ForMember(dest => dest.EventImg, opt => opt.MapFrom(src => src.EventImg));


            CreateMap<EventImg, EventImgDTO>();
            CreateMap<Company, ListCompanyDTO>();
            CreateMap<ListCompanyDTO, Company>();
            CreateMap<Event, ViewEventDTO>();
            CreateMap<ViewEventDTO, Event>();

            CreateMap<User, UserResponseDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<LoginUserRequest, User>();
            CreateMap<FeedbackDTO, FeedBack>();
            CreateMap<FeedBack, FeedbackDTO>();
            CreateMap<ResponseDTO, Response>();
            CreateMap<Response, ResponseDTO>();
            CreateMap<CreateBookingDto, Booking>();
            CreateMap<Booking, ViewDetailsBookingDto>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Event.Company.Name));
            CreateMap<User, UserDTO>();

            // Promotion
            CreateMap<Promotion, ViewDetailsPromotionDto>();
            CreateMap<CreatePromotionDto, Promotion>();
            CreateMap<UpdatePromotionDto, Promotion>();
            CreateMap<Promotion, UpdatePromotionDto>();

            // Thêm ánh xạ cho SubscriptionPackage
            CreateMap<SubscriptionPackageDTO, SubscriptionPackage>();
            CreateMap<SubscriptionPackage, SubscriptionPackageDTO>();

            // Thêm ánh xạ mới cho ListSubscriptionPackageDTO
            CreateMap<SubscriptionPackage, ListSubscriptionPackageDTO>();

        }
    }
}
