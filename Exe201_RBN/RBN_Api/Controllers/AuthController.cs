using AutoMapper;
using BusinessObject;
using BusinessObject.Dto.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace RBN_Api.Controllers
{
    [Route("api/[controller]")]
[ApiController]
    public class AuthController : ControllerBase
    {
        IMapper _mapper;
        IAuthService _authService;
        public AuthController(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest login)
        {
            var user = _mapper.Map<LoginUserRequest, User>(login);
            var result = await _authService.Login(user);
            return result;
        }
    }
}
