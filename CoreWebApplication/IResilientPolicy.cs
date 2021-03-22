using Polly;
using System.Net.Http;

namespace CoreWebApplication
{
    public interface IResilientPolicy
    {
        IAsyncPolicy<HttpResponseMessage> Policy { get; }
    }
}