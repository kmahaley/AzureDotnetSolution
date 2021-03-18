using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreWebApplication
{
    public interface IResilientPolicy
    {
        IAsyncPolicy<HttpResponseMessage> Policy { get; }
    }
}
