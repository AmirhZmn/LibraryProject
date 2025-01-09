using AspNetCoreRateLimit;

namespace ModularPatternTraining.Shared.Services
{

    public static class RateLimitingServiceRegistration
    {
        public static void AddRateLimitingServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(configuration.GetSection("RateLimitOptions"));

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            services.AddInMemoryRateLimiting();
        }
    }
}
