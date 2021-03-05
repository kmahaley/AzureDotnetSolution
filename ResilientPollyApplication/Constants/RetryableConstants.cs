using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ResilientPollyApplication.Constants
{
    public static class RetryableConstants
    {
        public static IEnumerable<HttpStatusCode> httpStatusCodesWorthRetrying = new List<HttpStatusCode> {
                   HttpStatusCode.RequestTimeout, // 408
                   HttpStatusCode.InternalServerError, // 500
                   HttpStatusCode.BadGateway, // 502
                   HttpStatusCode.ServiceUnavailable, // 503
                   HttpStatusCode.GatewayTimeout // 504
                };

        public const string PollyBasedNamedHttpClient = "PollyBasedNamedHttpClient";


    }
}
