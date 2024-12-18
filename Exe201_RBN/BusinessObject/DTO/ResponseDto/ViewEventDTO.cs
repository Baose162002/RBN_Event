﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.ResponseDto
{
    public class ViewEventDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string EventType { get; set; }
        public double Price { get; set; }
        public int MinCapacity { get; set; }
        public int MaxCapacity { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int CompanyId { get; set; }
        public string CreateBy { get; set; }
        public string CreateAt { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateAt { get; set; }
    }
}
