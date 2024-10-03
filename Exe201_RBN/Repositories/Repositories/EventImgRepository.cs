using BusinessObject;
using Microsoft.AspNetCore.Http;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
	public class EventImgRepository : IEventImgRepository
	{
        public async Task<int> UploadFile(EventImg eventImg)
        {
            var _context = new ApplicationDBContext();
			_context.EventImgs.Add(eventImg);
			await _context.SaveChangesAsync();
            return eventImg.Id;

        }
    }
}
