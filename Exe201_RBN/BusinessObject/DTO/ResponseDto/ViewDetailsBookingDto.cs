using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ResponseDto
{
    public class ViewDetailsBookingDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string Phone { get; set; }
        public DateTime BookingDay { get; set; }
        public string UserNote { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }

    }
}
