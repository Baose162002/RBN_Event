using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BusinessObject
{
    [Table("Event")]
    public class Event
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string EventType { get; set; }
        public string Price { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int CompanyId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }

        // Relationships
        public Company Company { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<EventImg> EventImages { get; set; }
        public ICollection<PromotionFee> PromotionFees { get; set; }
    }

}
