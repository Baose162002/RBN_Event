using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.RequestDto
{
    public class CreatePromotionDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DurationInDays { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
    }
}