﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.RequestDto
{
    public class CreateUserDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên của bạn")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập của bạn"),EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại của bạn"), Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập nơi ở của bạn")]
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
