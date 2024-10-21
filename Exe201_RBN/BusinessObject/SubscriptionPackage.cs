﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    [Table("SubscriptionPackage")]
    public class SubscriptionPackage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Required]
        public int DurationInDays { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsAvailable { get; set; } 

        public ICollection<Company> Companies { get; set; }
    }
}
