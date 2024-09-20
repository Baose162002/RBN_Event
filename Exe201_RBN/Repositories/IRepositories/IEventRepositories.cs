using Repositories.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IEventRepositories
    {
        Task<List<Event>> GetAllEvent();
    }
}
