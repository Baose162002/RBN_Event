using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    [Table("User")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
		public bool Status { get; set; }
		public int? RoleId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Relationships
        public ICollection<Company> Companies { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<FeedBack> FeedBacks { get; set; }



    }
}
