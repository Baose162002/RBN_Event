using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.RequestDto
{
    public class UpdateEventDto
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống.")]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Tên không được để trống.")]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Loại sự kiện không được để trống.")]
        [Display(Name = "Loại sự kiện")]
        public string EventType { get; set; }

        [Required(ErrorMessage = "Giá không được để trống.")]
        [Display(Name = "Giá")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Số lượng tối thiểu không được để trống.")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống.")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        public int Status { get; set; }
        public string CreateBy { get; set; }

        public string CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateAt { get; set; }
        public int EventImgId { get; set; }
    }
}
