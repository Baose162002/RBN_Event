using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.RequestDto
{
    public class CreateCompanyDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên của bạn")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập của bạn"), EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại của bạn"), Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập nơi ở của bạn")]
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }

        //company fields
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Mô tả một chút về công ty của bạn. Ví dụ: ABC là 1 công ty chuyên tổ chức sự kiện Teabreak")]
        public string CompanyDescription { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        [Required(ErrorMessage = "Yêu cầu cung cấp 1 hình dại diện cho công ty bạn")]
        public string Avatar { get; set; }
        public string TaxCode { get; set; }
    }
}
