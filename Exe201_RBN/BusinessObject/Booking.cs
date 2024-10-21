using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Phone { get; set; }
        public DateTime BookingDay { get; set; }
        public string UserNote { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Event Event { get; set; }
    }

}
