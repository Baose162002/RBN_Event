using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    [Table("EventImg")]
    public class EventImg
    {
        [Key]
        public int Id { get; set; }   
        public string ImageUrl { get; set; }       
        public DateTime DateUpLoad { get; set; }
    }
}
