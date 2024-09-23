using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    [Table("Commission")]
    public class CommissionFee
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime PaidAt { get; set; }
        public string PaymentMethod { get; set; }
        public int Status { get; set; }
        public int CompanyId { get; set; }
        public int BookingId { get; set; }
        public DateTime CreateAt { get; set; }

        // Navigation Properties
        public Company Company { get; set; }
        public Booking Booking { get; set; }
    }

}
