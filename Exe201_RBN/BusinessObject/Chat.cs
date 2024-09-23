using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    [Table("Chat")]
    public class Chat
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }

        public int UserId { get; set; }

        // Navigation Properties

        public User User { get; set; }
        public ICollection<Message> Messages { get; set; }
    }

}
