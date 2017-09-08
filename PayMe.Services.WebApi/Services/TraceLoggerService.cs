using Microsoft.Owin.Logging;
using System;
using System.Diagnostics;

namespace PayMe.Services.WebApi.Services
{
    public class TraceLoggerService : ILogger
    {

        private readonly string _componentName;

        public TraceLoggerService(string component = Startup.AppName)
        {
            _componentName = component;
        }

        public bool WriteCore(TraceEventType eventType, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            Trace.WriteLine($"{eventType} from {_componentName}: {state}");
            return true;
        }

        public static string WriteAndFormat(object state, Exception exception)
        {
            return $"ERROR: {state} | {exception.Message}";
        }

    }
}