using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DistributedCacheApplication.HealthCheck
{
    public class HealthCheckOptionsCreator
    {
        public static HealthCheckOptions GetHealthCheckOptions()
        {
            var healthOptions = new HealthCheckOptions();
            healthOptions.ResultStatusCodes = GetHealthStatusDictionary();
            healthOptions.ResponseWriter = CustomResponseWriter;
            return healthOptions;
        }

        public static Task CustomResponseWriter(HttpContext context, HealthReport healthReport)
        {
            context.Response.ContentType = "application/json";

            var result = JsonConvert.SerializeObject(new
            {
                status = healthReport.Status.ToString(),
                healthData = healthReport.Entries
                    .Select(e =>
                        new
                        {
                            member = e.Key,
                            status = e.Value.Status.ToString(),
                            data = string.Join(", ", e.Value.Data.Select(kv => $"{kv.Key} : {kv.Value}").ToArray()),
                            description = e.Value.Description
                        })
            });

            return context.Response.WriteAsync(result);

        }

        public static Dictionary<HealthStatus, int> GetHealthStatusDictionary()
        {
            return new Dictionary<HealthStatus, int>()
            {
                { HealthStatus.Healthy, 200 },
                { HealthStatus.Unhealthy, 200 },
                { HealthStatus.Degraded, 200 }
            };
        }
    }

    public class RedisHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            //ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("string");
            //var db = _redis.GetDatabase();
            //db.Ping();

            var isHealthy = true;

            var data = new Dictionary<string, Object>()
            {
                { "RedisHost", "localhost" },
                { "RedisPort", "6379" }
            };

            if (isHealthy)
            {
                return Task.FromResult(
                    new HealthCheckResult(HealthStatus.Healthy, "Redis is healthy", null, data));
            }

            return Task.FromResult(
                new HealthCheckResult(HealthStatus.Unhealthy, "Redis is unhealthy", null, data));
        }
    }

    public class SqlHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            //var connection = new SqlConnection("");
            //connection.Open();

            var isHealthy = false;

            var data = new Dictionary<string, Object>()
            {
                { "SqlHost", "localhost" },
                { "SqlPort", "1443" }
            };

            if (isHealthy)
            {
                return Task.FromResult(
                    new HealthCheckResult(HealthStatus.Healthy, "Sql is healthy", null, data));
            }

            return Task.FromResult(
                new HealthCheckResult(HealthStatus.Unhealthy, "Sql is unhealthy", null, data));
        }
    }
}
