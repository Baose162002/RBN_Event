using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO.RequestDto
{
    public class UploadImageResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public int ImageId { get; set; }
    }

}
