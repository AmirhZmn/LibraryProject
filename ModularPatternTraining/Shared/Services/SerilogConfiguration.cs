using Serilog;

namespace ModularPatternTraining.Shared.Services
{
    public class SerilogConfiguration
    {
        public static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            
            var logConfiguration = new LoggerConfiguration();
            switch (environment)
            {
                case "Development":
                    logConfiguration = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .Enrich.WithThreadId()
                        .MinimumLevel.Information()
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
                    break;
                case "Production":
                    logConfiguration = new LoggerConfiguration()
                        .MinimumLevel.Warning()
                        .WriteTo.File(
                            path: "Logs/log-.json",
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 7,
                            formatter: new Serilog.Formatting.Json.JsonFormatter()
                        ); // LoggerConfiguration
                    break;
            }


            Log.Logger = logConfiguration.CreateLogger();
        }
    }
}
