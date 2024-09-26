using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class ResponseDTO
    {
        public string Comment { get; set; }
        public DateTime ResponseDate { get; set; }
        public int FeedBackId { get; set; }
        public int CompanyId { get; set; }
    }
}
