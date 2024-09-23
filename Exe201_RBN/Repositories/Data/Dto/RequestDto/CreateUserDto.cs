using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.Dto.RequestDto
{
    public class CreateUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
