using AutoMapper;
using BusinessObject;
using BusinessObject.Dto.RequestDto;
using BusinessObject.Dto.ResponseDto;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using Repositories.Repositories;
using Repositories.Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepo;
        private readonly IBaseRepository<Company> _companyRepo;
        private readonly ISendMailService _sendMail;
        private readonly IMapper _mapper;
        public UserService(IBaseRepository<User> userRepo, IBaseRepository<Company> companyRepo,
            IMapper mapper, ISendMailService sendMail)
        {
            _userRepo = userRepo;
            _companyRepo = companyRepo;
            _mapper = mapper;
            _sendMail = sendMail;
        }
        public async Task<UserDTO> GetUserNameById(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
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
        public async Task<List<UserResponseDto>> SearchUser(int? id, string? name, string? mail, string? phone,
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
                    Name = createUserDto.Name,
                    Email = createUserDto.Email,
                    Phone = createUserDto.Phone,
                    Address = createUserDto.Address,
                    Status = true,
                    RoleId = 3,
                    CreatedAt = DateTime.Now,
                    Password = "12345"
                };

                await _userRepo.CreateAsync(createdUser);

                await _sendMail.SendMailToGeneratedUser(createdUser.Email);

                return _mapper.Map<UserResponseDto>(createdUser);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateCompanyRole(CreateCompanyDto createCompanyDto)
        {
            try
            {
                if (CheckEmailExist(createCompanyDto.Email))
                {
                    throw new Exception("Email đã tồn tại.");
                }
                if (CheckPhoneExist(createCompanyDto.Phone))
                {
                    throw new Exception("Số điện thoại đã tồn tại.");
                }

                var createdUser = new User
                {
                    Name = createCompanyDto.Name,
                    Email = createCompanyDto.Email,
                    Phone = createCompanyDto.Phone,
                    Address = createCompanyDto.Address,
                    Status = false,
                    RoleId = 4,
                    CreatedAt = DateTime.Now,
                    Password = "12345"
                };
                await _userRepo.CreateAsync(createdUser);

                //tạo company sau khi tạo user
                var company = new Company
                {
                    UserId = createdUser.Id, // liên kết với User
                    Name = createdUser.Name,
                    Description = createCompanyDto.CompanyDescription,
                    Address = createdUser.Address,
                    Phone = createdUser.Phone,
                    Avatar = createCompanyDto.Avatar,
                    TaxCode = createCompanyDto.TaxCode,
                };
                await _companyRepo.CreateAsync(company);
                createdUser.Status = true;
                await _userRepo.UpdateAsync(createdUser);
                _sendMail.SendMailToGeneratedUser(createCompanyDto.Email);

                //await _userRepo.BeginTransactionAsync();

                //var createdUser = new User
                //{
                //    Name = createCompanyDto.Name,
                //    Email = createCompanyDto.Email,
                //    Phone = createCompanyDto.Phone,
                //    Address = createCompanyDto.Address,
                //    Status = true,
                //    RoleId = 4,
                //    CreatedAt = DateTime.Now,
                //    Password = "12345"
                //};
                // await _userRepo.CreateAsync(createdUser);
                ////tạo company sau khi tạo user
                //var company = new Company
                //{
                //    UserId = createdUser.Id, // liên kết với User
                //    Name = createdUser.Name,
                //    Description = createCompanyDto.CompanyDescription,
                //    Address = createdUser.Address,
                //    Phone = createdUser.Phone,
                //    Avatar = createCompanyDto.Avatar,
                //    TaxCode = createCompanyDto.TaxCode,
                //};
                // await _companyRepo.CreateAsync(company);

                //_sendMail.SendMailToGeneratedUser(createCompanyDto.Email);

                //// Commit giao dịch nếu mọi thứ thành công
                //await _userRepo.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                await _userRepo.RollbackTransactionAsync();
                Console.WriteLine(ex.InnerException?.Message);
                throw new Exception("An error occurred while creating the company role.", ex);
            }
        }

        ////
        ///
        public async Task<int> CreateCompanyRoleFE(CreateCompanyDto createCompanyDto)
        {
            try
            {
                // Kiểm tra các trường bắt buộc
                if (string.IsNullOrWhiteSpace(createCompanyDto.Name))
                    throw new Exception("Tên công ty là bắt buộc.");

                if (string.IsNullOrWhiteSpace(createCompanyDto.Email) || !IsValidEmail(createCompanyDto.Email))
                    throw new Exception("Email hợp lệ là bắt buộc.");

                if (string.IsNullOrWhiteSpace(createCompanyDto.Phone) || !IsValidPhoneNumber(createCompanyDto.Phone))
                    throw new Exception("Số điện thoại hợp lệ là bắt buộc.");

                if (string.IsNullOrWhiteSpace(createCompanyDto.CompanyAddress))
                    throw new Exception("Địa chỉ công ty là bắt buộc.");

                if (string.IsNullOrWhiteSpace(createCompanyDto.TaxCode))
                    throw new Exception("Mã số thuế là bắt buộc.");

                // Kiểm tra nếu email và số điện thoại đã tồn tại
                if (CheckEmailExist(createCompanyDto.Email))
                    throw new Exception("Email đã tồn tại.");

                if (CheckPhoneExist(createCompanyDto.Phone))
                    throw new Exception("Số điện thoại đã tồn tại.");

                // Tạo người dùng
                var createdUser = new User
                {
                    Name = createCompanyDto.Name,
                    Email = createCompanyDto.Email,
                    Phone = createCompanyDto.Phone,
                    Address = createCompanyDto.Address,
                    Status = false,
                    RoleId = 4,
                    CreatedAt = DateTime.Now,
                    Password = GenerateRandomPassword() // Tạo mật khẩu ngẫu nhiên
                };
                await _userRepo.CreateAsync(createdUser);

                // Tạo công ty
                var company = new Company
                {
                    UserId = createdUser.Id,
                    Name = createCompanyDto.CompanyName,
                    Description = createCompanyDto.CompanyDescription,
                    Address = createCompanyDto.CompanyAddress,
                    Phone = createCompanyDto.CompanyPhone,
                    Avatar = createCompanyDto.Avatar, // Avatar có thể null
                    TaxCode = createCompanyDto.TaxCode,
                };
                await _companyRepo.CreateAsync(company);

                createdUser.Status = true;
                await _userRepo.UpdateAsync(createdUser);
                await _sendMail.SendMailToGeneratedUser(createCompanyDto.Email);

                return company.Id;
            }
            catch (Exception ex)
            {
                await _userRepo.RollbackTransactionAsync(); 
                Console.WriteLine(ex.Message); 
                throw new Exception("Đã xảy ra lỗi khi tạo vai trò công ty: " + ex.Message);
            }
        }

        private bool IsValidEmail(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }

        private bool IsValidPhoneNumber(string phone)
        {
            return phone.Length == 10 && phone.StartsWith("0") && phone.All(char.IsDigit);
        }


        private string GenerateRandomPassword()
        {
            // Implement logic to generate a random password
            // For example:
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }

/*        public async Task<UserResponseDto> CreateUserForCompanyAsync(CreateUserDto createUserDto)
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
                    Name = createUserDto.Name,
                    Email = createUserDto.Email,
                    Phone = createUserDto.Phone,
                    Address = createUserDto.Address,
                    Status = false,
                    RoleId = 4, // Assuming 4 is the role ID for company users
                    CreatedAt = DateTime.Now,
                    Password = "12345" // Consider generating a random password here
                };
                await _userRepo.CreateAsync(createdUser);

                // Map the created User to UserResponseDto and return it
                return _mapper.Map<UserResponseDto>(createdUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }*/

        public async Task InactiveUser(int id)
        {
            var existedUser = await _userRepo.GetByIdAsync(id);
            if (existedUser == null)
            {
                throw new Exception("Người dùng không tồn tại.");
            }

            if(existedUser.Status == true)
            {
                existedUser.Status = false;
            }else if(existedUser.Status == false) { existedUser.Status = true; }

            await _userRepo.UpdateAsync(existedUser);
        }

        public async Task DeleteUser(int id)
        {
            var existedUser = await _userRepo.GetByIdAsync(id);
            if (existedUser == null)
            {
                throw new Exception("Người dùng không tồn tại.");
            }

            var remove = await _userRepo.RemoveAsync(existedUser);
        }
        public async Task<UserResponseDto> UpdateUser(UpdateUserDto updateUserDto)
        {
            var existedUser = await _userRepo.GetByIdAsync(updateUserDto.Id);
            if (existedUser != null)
            {
                existedUser.Name = updateUserDto.Name;
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

        public async Task ChangePassword(ChangePasswordDto changePassword)
        {
            var user = await _userRepo.GetByIdAsync(changePassword.Id);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại");
            }
            if (user.Password != changePassword.OldPassword)
            {
                throw new Exception("Sai mật khẩu cũ");
            }
            if (string.IsNullOrEmpty(changePassword.NewPassword) || string.IsNullOrEmpty(changePassword.ConfirmPassword))
            {
                throw new Exception("Mật khẩu mới và xác nhận mật khẩu không được để trống.");
            }
            if(changePassword.ConfirmPassword != changePassword.NewPassword)
            {
                throw new Exception("Xác nhận mật khẩu phải đúng với mật khẩu mới");
            }

            user.Password = changePassword.NewPassword;
            
            await _userRepo.UpdateAsync(user);
        }
        public async Task ForgotPassword(string email, string phone)
        {
            if (email == null)
            {
                throw new Exception("Vui lòng nhập email");
            }
            if (phone == null)
            {
                throw new Exception("Vui lòng nhập số điện thoại");
            }
            var correctEmailAccount = _userRepo.Get()
                .Where(x => x.Email == email && x.Phone == phone)
                .FirstOrDefault();
            if (correctEmailAccount == null) 
            {
                throw new Exception("Sai email hoặc số điện thoại tài khoản");
            }
                _sendMail.SendMailToGeneratedUser(email);
            
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
