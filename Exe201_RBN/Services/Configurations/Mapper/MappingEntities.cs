using AutoMapper;
using BusinessObject;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;

namespace Services.Configurations.Mapper
{
    public class MappingEntities : Profile
    {
        public MappingEntities()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<CreateUserDto, User>();
        }
    }
}
