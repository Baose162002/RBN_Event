using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Dto.RequestDto
{
    public class CreateCompanyDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên công ty là bắt buộc")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Định dạng số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Tên công ty là bắt buộc")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Mô tả công ty là bắt buộc")]
        public string CompanyDescription { get; set; }

        [Required(ErrorMessage = "Địa chỉ công ty là bắt buộc")]
        public string CompanyAddress { get; set; }

        [Required(ErrorMessage = "Số điện thoại công ty là bắt buộc")]
        [Phone(ErrorMessage = "Định dạng số điện thoại công ty không hợp lệ")]
        public string CompanyPhone { get; set; }

        [Required(ErrorMessage = "Avatar là bắt buộc")]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "Mã số thuế là bắt buộc")]
        public string TaxCode { get; set; }
    }
}
