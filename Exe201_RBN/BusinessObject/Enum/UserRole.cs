using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Enum
{
    public class Enum
    {
        public enum UserRole
        {
            Admin = 1,
            Staff = 2,
            User = 3,
            Company = 4,
        }
        public enum BookingStatus
        {
            Unprocessed = 0,
            Processed = 1,
        }
    }
}