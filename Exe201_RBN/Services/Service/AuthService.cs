using BusinessObject;
using BusinessObject.Enum;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseRepository<User> _userRepo;
        private readonly IConfiguration _configuration;
        public AuthService(IBaseRepository<User> userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _configuration = configuration;
        }

        public async Task<IActionResult> Login(User user)
        {
            var checkUser = await _userRepo.Get()
                .Where(x => x.Email == user.Email && x.Password == user.Password)
                .FirstOrDefaultAsync();
            if (checkUser == null)
            {
                return new UnauthorizedResult();
            }
            else
            {
                var role = (BusinessObject.Enum.Enum.UserRole)checkUser.RoleId;
                // get token
                var authClaims = new List<Claim>
                {
                    new Claim("CompanyId", checkUser.Id.ToString()),


                    new Claim(ClaimTypes.Name, checkUser.Name),
                    new Claim(ClaimTypes.Email, checkUser.Email),
                    new Claim(ClaimTypes.Role, role.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, checkUser.Id.ToString())
                };
                //Tạo token
                var token = GenerateToken(authClaims);

                //Return token cho client
                return new OkObjectResult(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    Role = role,
                    expiration = token.ValidTo,
                    UserId = checkUser.Id
                });
            }
        }
        private JwtSecurityToken GenerateToken(IEnumerable<Claim> authClaims)
        {
            // kí token với SecretKey
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            //tạo token
            var token = new JwtSecurityToken
                (
                    issuer: _configuration["JWT:ValidIssuer"], // Who create Token
                    audience: _configuration["JWT:ValidAudience"], // Who claim Token
                    expires: DateTime.Now.AddDays(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
    }
}