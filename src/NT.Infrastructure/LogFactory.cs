using Microsoft.Extensions.Logging;

namespace NT.Infrastructure
{
    public class LogFactory
    {
        public ILogger GetLogger<TType>() where TType : class
        {
            var loggerFactory = new LoggerFactory().AddConsole().AddDebug();
            return loggerFactory.CreateLogger<TType>();
        }

        public static ILogger GetLogInstance<TType>() where TType : class
        {
            return new LogFactory().GetLogger<TType>();
        }
    }
}