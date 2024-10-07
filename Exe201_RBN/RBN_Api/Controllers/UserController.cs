using BusinessObject;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Services.IService;
using Services.Service;

namespace RBN_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService ??= userService;
        }
        [HttpGet]
        public async Task<List<UserResponseDto>> getAllUser()
        {
            var users = await _userService.getAllUser();
            return users;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user;
        }
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            await _userService.CreateUserAsync(createUserDto);
            return Ok(new {message = "Tạo tài khoản thành công, vui lòng kiểm tra email để lấy mật khẩu!"});
        }

        /*        [HttpPost("create-userFE")]
                public async Task<IActionResult> CreateUserFE(CreateUserDto createUserDto)
                {
                    try
                    {
                        UserResponseDto createdUser = await _userService.CreateUserForCompanyAsync(createUserDto);
                        return Ok(createdUser);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { message = ex.Message });
                    }
                }*/


        [HttpPost("create-companyFE")]
        public async Task<IActionResult> CreateCompanyFE(CreateCompanyDto createCompanyDto)
        {
            try
            {
                var userId = await _userService.CreateCompanyRoleFE(createCompanyDto);
                var createdUser = await _userService.GetUserByIdAsync(userId);

                return Ok(new
                {
                    Status = "success",
                    Message = "Company created successfully",
                    User = createdUser
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("search")]
        public async Task<List<UserResponseDto>> SearchUser(int? id, string? name, string? mail, string? phone,
                                                    string? address, int? roleId, DateTime? createAt)
        {
            var user = await _userService.SearchUser(id, name, mail, phone, address, roleId, createAt);
            return user;
        }
        [HttpPut]
        public async Task<ActionResult<UserResponseDto>> UpdateUser(UpdateUserDto updateUserDto)
        {
            var updatedUser = await _userService.UpdateUser(updateUserDto);
            return updatedUser;
        }
        [HttpPut("inactive/{id}")]
        public async Task<IActionResult> InactiveUser(int id)
        {

            await _userService.InactiveUser(id);
            return Ok(new { message = "Trạng thái người dùng đã được cập nhật." });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return Ok(new { message = "Người dùng đã được xóa thành công." });
        }

        [HttpGet("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email, string phone)
        {
            await _userService.ForgotPassword(email, phone);
            return Ok(new { message = "Mật khẩu đã được gửi đến mail của bạn" });
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            await _userService.ChangePassword(changePasswordDto);
            return Ok(new { message = "Thay đổi mật khẩu thành công" });
        }

        [HttpPost("create-company")]
        public async Task<IActionResult> CreateCompany(CreateCompanyDto createCompanyDto)
        {
            await _userService.CreateCompanyRole(createCompanyDto);
            return Ok(new { message = "Tạo công ty thành công" });
        }    
    }
}
