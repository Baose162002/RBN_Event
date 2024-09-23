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
            var user = await _userRepo.Get().Where(x => x.Email.ToLower().Contains(email.ToLower())).FirstOrDefaultAsync();
            if (user == null) throw new Exception("Not Found");

            var from = new MailboxAddress(name: "", address: "tivip1216vn@gmail.com");
            var to = new MailboxAddress(name: "", address: user.Email.ToLower());


            var msj = new MimeMessage();
            msj.From.Add(from);
            msj.To.Add(to);
            msj.Subject = "Confirm your account in RBN Event";
            msj.Body = new TextPart(TextFormat.Html)
            {

                Text = $"<h1>Hi {user.Name}</h1>" +
                $"<h2>Welcome to RBN Event</h2>" +
                $"<p>We are glad for your joining into our system. We hope you will happy to use our website<br></br>Here is your account:</p>" +
                $"<p>Your Account: {email} <br></br>Your Password: {user.Password}</p>"
            };

            var client = new MailKit.Net.Smtp.SmtpClient();

            client.Connect(host: "smtp.gmail.com",
                           port: 587,
                           options: MailKit.Security.SecureSocketOptions.StartTls);



            client.Authenticate("tivip1216vn@gmail.com", "nwlkhxscvhmanubs");

            client.Send(msj);

            client.Disconnect(true);
        }
    }
}
