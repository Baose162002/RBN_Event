﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.ResponseDto
{
    public class ViewDetailsBookingDto
    {
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
        public string EventName { get; set; }
        public string CompanyName { get; set; }

    }
}
