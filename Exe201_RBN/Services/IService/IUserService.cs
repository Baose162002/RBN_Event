
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> getAllUser();
        Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<List<UserResponseDto>> SearchUser(int? id, string? username, string? name, string? mail, string? phone,
                                                    string? address, int? roleId, DateTime? createAt);
        Task<UserResponseDto> UpdateUser(UpdateUserDto updateUserDto);
    }
}
