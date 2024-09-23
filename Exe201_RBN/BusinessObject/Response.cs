using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Response
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime ResponseDate { get; set; }
        public int FeedBackId { get; set; }
        public int CompanyId { get; set; }

        // Navigation Properties
        public FeedBack FeedBack { get; set; }
        public Company Company { get; set; }
    }
}
