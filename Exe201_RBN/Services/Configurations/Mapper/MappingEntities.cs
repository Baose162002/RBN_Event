using AutoMapper;
using Repositories.Data.Dto.ResponseDto;
using Repositories.Data;
using Repositories.Data.Dto.RequestDto;

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
