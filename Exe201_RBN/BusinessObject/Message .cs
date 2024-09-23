using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    [Table("Message")]
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }

        public int ChatID { get; set; }

        // Navigation Properties

        public Chat Chat { get; set; }
    }

}
