using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class FeedbackDTO
    {
        public string Comment { get; set; }
        public DateTime FeedbackDate { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
    }
}
