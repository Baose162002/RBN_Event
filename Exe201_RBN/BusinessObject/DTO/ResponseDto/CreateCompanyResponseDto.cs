using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.ResponseDto
{
    public class CreateCompanyResponseDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public int CompanyId { get; set; }
    }
}
