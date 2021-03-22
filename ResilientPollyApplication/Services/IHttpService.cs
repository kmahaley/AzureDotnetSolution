using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Services
{
    public interface IHttpService
    {
        string GetServiceName();

        Task<List<string>> TestHttpCallWithPollyBasedFramework();

        Task<List<string>> TestHttpCallWithPollyBasedFrameworkDuplicate();
    }
}