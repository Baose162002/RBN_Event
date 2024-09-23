using Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    [Table("FeedBack")]
    public class FeedBack
    {
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime FeedbackDate { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Company Company { get; set; }
        public ICollection<Response> Responses { get; set; }
    }
}
