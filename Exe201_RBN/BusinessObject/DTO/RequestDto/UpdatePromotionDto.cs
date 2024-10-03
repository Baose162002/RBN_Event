using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Dto.RequestDto
{
    public class UpdatePromotionDto
    {
        [Required]
        public int Id { get; set; }

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
