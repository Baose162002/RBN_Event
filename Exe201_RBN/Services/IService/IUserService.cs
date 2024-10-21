
using BusinessObject;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO;
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
        Task<UserDTO> GetUserNameById(int id);
        Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<List<UserResponseDto>> SearchUser(int? id, string? name, string? mail, string? phone,
                                                    string? address, int? roleId, DateTime? createAt);
        Task<UserResponseDto> UpdateUser(UpdateUserDto updateUserDto);
        Task DeleteUser(int id);
        Task InactiveUser(int id);
        Task ChangePassword(ChangePasswordDto changePassword);
        Task ForgotPassword(string email, string phone);
        Task CreateCompanyRole(CreateCompanyDto createCompanyDto);
        Task<int> CreateCompanyRoleFE(CreateCompanyDto createCompanyDto);
/*        Task<UserResponseDto> CreateUserForCompanyAsync(CreateUserDto createUserDto);
*/    }
}
