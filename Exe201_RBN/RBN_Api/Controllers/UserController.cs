using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using Microsoft.AspNetCore.Mvc;

using Services.IService;

namespace RBN_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController
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
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserDto createUserDto)
        {
            var createdUser = await _userService.CreateUserAsync(createUserDto);
            return createdUser;
        }

        [HttpGet("search")]
        public async Task<List<UserResponseDto>> SearchUser(int? id, string? username, string? name, string? mail, string? phone,
                                                    string? address, int? roleId, DateTime? createAt)
        {
            var user = await _userService.SearchUser(id, username, name, mail, phone, address, roleId, createAt);
            return user;
        }
        [HttpPut]
        public async Task<ActionResult<UserResponseDto>> CreateUser(UpdateUserDto updateUserDto)
        {
            var updatedUser = await _userService.UpdateUser(updateUserDto);
            return updatedUser;
        }
    }
}
