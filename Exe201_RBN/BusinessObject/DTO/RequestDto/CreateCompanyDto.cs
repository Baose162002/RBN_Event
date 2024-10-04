using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Dto.RequestDto
{
    public class CreateCompanyDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Company Description is required")]
        public string CompanyDescription { get; set; }

        [Required(ErrorMessage = "Company Address is required")]
        public string CompanyAddress { get; set; }

        [Required(ErrorMessage = "Company Phone is required")]
        [Phone(ErrorMessage = "Invalid company phone number format")]
        public string CompanyPhone { get; set; }

        public string Avatar { get; set; }

        [Required(ErrorMessage = "Tax Code is required")]
        public string TaxCode { get; set; }
    }
}