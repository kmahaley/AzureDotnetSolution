using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApplication.Services
{
    public interface IBookService
    {
        public Task ProcessTasksInBackgroundAsync();
    }
}
