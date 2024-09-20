using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data
{
    [Table("PromotionFee")]
    public class PromotionFee
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PromotionType { get; set; }
        public string Description { get; set; }
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
        public int? EventId { get; set; }
        public int? CompanyId { get; set; }

        // Navigation Properties
        public Event Event { get; set; }
        public Company Company { get; set; }
    }

}
