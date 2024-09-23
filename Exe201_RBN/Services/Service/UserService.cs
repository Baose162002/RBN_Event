using AutoMapper;
using Repositories.Data;
using Repositories.Data.Dto.RequestDto;
using Repositories.Data.Dto.ResponseDto;
using Repositories.Repositories;
using Repositories.Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepo;
        private readonly IMapper _mapper;
        public UserService(IBaseRepository<User> userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }
        public async Task<List<UserResponseDto>> getAllUser()
        {
            var users = await _userRepo.GetAllAsync();
            return _mapper.Map<List<UserResponseDto>>(users);
        }
        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            return _mapper.Map<UserResponseDto>(user);
        }
        public async Task<List<UserResponseDto>> SearchUser(int? id, string? username, string? name, string? mail, string? phone,
                                                    string? address, int? roleId, DateTime? createAt)
        {
            var users = await _userRepo.GetAllAsync();
            if(users == null)
            {
                throw new Exception("No data");
            }
            if (id.HasValue) 
            {
                users = users.Where(x => x.Id == id.Value).ToList();
            }
            if (!string.IsNullOrEmpty(username)) 
            {
                users = users.Where(x => x.UserName.ToLower().Contains(username.ToLower())).ToList();            
            }
            if (!string.IsNullOrEmpty(name)) 
            {
                users = users.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(mail))
            {
                users = users.Where(x => x.Email.ToLower().Contains(mail.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(phone))
            {
                users = users.Where(x => x.Phone.ToLower().Contains(phone.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(address))
            {
                users = users.Where(x => x.Address.ToLower().Contains(address.ToLower())).ToList();
            }
            if (roleId.HasValue)
            {
                users = users.Where(x => x.RoleId == roleId.Value).ToList();
            }
            if(createAt != null)
            {
                users = users.Where(x => x.CreatedAt == createAt.Value).ToList();
            }
            var userList = users.ToList();
            if(userList == null)
            {
                throw new Exception("No data");
            }

            return _mapper.Map<List<UserResponseDto>>(userList);
        }
        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                if (CheckEmailExist(createUserDto.Email))
                {
                    throw new Exception("Email đã tồn tại.");
                }
                if (CheckPhoneExist(createUserDto.Phone))
                {
                    throw new Exception("Số điện thoại đã tồn tại.");
                }
                var createdUser = new User
                {
                    UserName = createUserDto.UserName,
                    Name = createUserDto.Name,
                    Email = createUserDto.Email,
                    Phone = createUserDto.Phone,
                    Address = createUserDto.Address,
                    RoleId = createUserDto.RoleId,
                    CreatedAt = DateTime.Now,
                    Password = "12345" 
                };

                await _userRepo.CreateAsync(createdUser);

                return _mapper.Map<UserResponseDto>(createdUser);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<UserResponseDto> UpdateUser(UpdateUserDto updateUserDto)
        {
            var existedUser = await _userRepo.GetByIdAsync(updateUserDto.Id);
            if (existedUser != null)
            {
                existedUser.Name = updateUserDto.Name;
                existedUser.Password = updateUserDto.Password;
                existedUser.Phone = updateUserDto.Phone;
                if (CheckPhoneExist(existedUser.Phone))
                {
                    throw new Exception("Số điện thoại đã tồn tại");
                }
                existedUser.Address = updateUserDto.Address;
            }
                await _userRepo.UpdateAsync(existedUser);

                return _mapper.Map<UserResponseDto>(existedUser);
        }

        private bool CheckEmailExist(string email)
        {
            var result = _userRepo.Get().Any(x => x.Email.ToLower().Trim() == email.ToLower().Trim());
            return result;
        }

        private bool CheckPhoneExist(string phone)
        {
            var result = _userRepo.Get().Any(x => x.Phone.Trim() == phone.Trim());
            return result;
        }
    }
}
