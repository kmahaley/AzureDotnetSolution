using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Services
{
    public interface IHttpService
    {
        Task TestHttpCallWithPollyBasedFramework();
    }
}
