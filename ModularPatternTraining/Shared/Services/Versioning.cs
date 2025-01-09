using Asp.Versioning;


namespace ModularPatternTraining.Shared.Services
{
    public static class ApiVersioningSetup
    {
        public static void AddApiVersioningConfiguration(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1); //More explicit versioning
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader("X-Api-Version"));
                })
                .AddMvc()// This is needed for controllers. Consider replacing with AddControllersWithViews() if you're using views
                .AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VV"; //More consistent formatting
                    options.SubstituteApiVersionInUrl = true;
                });
        }
    }

}


