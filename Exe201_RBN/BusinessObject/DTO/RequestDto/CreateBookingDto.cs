using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.RequestDto
{
    public class CreateBookingDto
    {
        [Required(ErrorMessage = "Yêu cầu nhập email của bạn")]
        public string Email { get; set; }
        public string FullName { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập nơi ở của bạn")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại của bạn"),Phone]
        public string Phone { get; set; }
        public string UserNote { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}
