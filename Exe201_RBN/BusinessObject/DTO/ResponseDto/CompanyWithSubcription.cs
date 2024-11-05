using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.ResponseDto
{
    public class CompanyWithSubcription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime? SubscriptionStartTime { get; set; }
        public DateTime? SubscriptionEndTime { get; set; }

        public ViewSubscriptionPackageDTO SubscriptionPackage { get; set; }
    }
}
