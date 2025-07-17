using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EnsektechTest.Fixtures
{
    public class ConfigurationFixture
    {
        public IConfiguration? Config { get; }
        public ConfigurationFixture()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            //Config = builder.AddStandardProviders().Build();
        }

        public static ILogger Logger(string testName)
        {
            using ILoggerFactory factory =
                LoggerFactory.
                Create(builder => builder
                .AddLog4Net(new Log4NetProviderOptions
                {
                    Log4NetConfigFileName = "log4net.config",
                    Watch = true
                })
                .SetMinimumLevel(LogLevel.Information));
            return factory.CreateLogger(testName);
        }
    }
}
