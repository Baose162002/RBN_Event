using BusinessObject;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using Repositories.Repositories.IRepositories;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class SendMailService : ISendMailService
    {
        private IBaseRepository<User> _userRepo;
        public SendMailService(IBaseRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task SendMailToGeneratedUser(string email)
        {
            try
            {
                var user = await _userRepo.Get().Where(x => x.Email.ToLower().Contains(email.ToLower())).FirstOrDefaultAsync();
                if (user == null) throw new Exception("Không tìm thấy người dùng");

                var from = new MailboxAddress("", "tivip1216vn@gmail.com");
                var to = new MailboxAddress("", user.Email.ToLower());

                var msj = new MimeMessage();
                msj.From.Add(from);
                msj.To.Add(to);
                msj.Subject = "Xác nhận tài khoản của bạn với RBN Event";
                msj.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<h1>Xin chào {user.Name}</h1>" +
                           $"<h2>Chào mừng đến với RBN Event</h2>" +
                           $"<p>Tài khoản của bạn: {email} <br>Mật khẩu: {user.Password}</p>"
                };

                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("tivip1216vn@gmail.com", "nwlkhxscvhmanubs");

                await client.SendAsync(msj);
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Không thể gửi email đến {email}: {ex.Message}");
                throw;
            }
        }

    }
}
