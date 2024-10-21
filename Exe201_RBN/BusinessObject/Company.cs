using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    [Table("Company")]
    public class Company
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string TaxCode { get; set; }
        public int UserId { get; set; }
        public int? SubscriptionPackageId { get; set; }

        public DateTime? SubscriptionStartTime { get; set; }
        public DateTime? SubscriptionEndTime { get; set; }
        public bool IsActive { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<FeedBack> FeedBacks { get; set; }
        public ICollection<PromotionFee> PromotionFees { get; set; }

        public SubscriptionPackage SubscriptionPackage { get; set; }
    }

}
