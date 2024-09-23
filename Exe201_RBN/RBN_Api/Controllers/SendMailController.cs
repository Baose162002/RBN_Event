using Microsoft.AspNetCore.Mvc;
using Services.IService;

namespace RBN_Api.Controllers
{
    [ApiController]
    public class SendMailController : ControllerBase
    {
        private readonly ISendMailService _service;
        public SendMailController(ISendMailService mailService)
        {
            this._service = mailService;
        }
        [HttpGet("send-email/send-password-for-creating")]
        public async Task<IActionResult> SendMail(string email)
        {
            await _service.SendMailToGeneratedUser(email);
            return Ok();
        }
    }
}
