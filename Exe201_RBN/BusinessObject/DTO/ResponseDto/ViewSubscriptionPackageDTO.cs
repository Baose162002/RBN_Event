using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.ResponseDto
{
    public class ViewSubscriptionPackageDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int DurationInDays { get; set; }

        public string Description { get; set; }

        public bool IsAvailable { get; set; }
    }
}
