using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    [Table("PromotionFee")]
    public class PromotionFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public int Status { get; set; }  // Trạng thái gói (0: Chưa thanh toán, 1: Đã thanh toán)

        public int? EventId { get; set; }  

        public int? CompanyId { get; set; }  

        public int PromotionId { get; set; }  
        public Promotion Promotion { get; set; }

        public Event Event { get; set; }
        public Company Company { get; set; }
    }
}
