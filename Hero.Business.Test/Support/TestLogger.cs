using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Hero.Business.Test.Support
{
    internal class TestLogger : ILogger
    {
        private readonly ITestOutputHelper outputHelper;

        public TestLogger(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = string.Empty;

            if (formatter != null)
            {
                message = formatter(state, exception);
            }
            else
            {
                //message = LogFormatter.Formatter(state, exception);
            }

            this.outputHelper.WriteLine(message);
        }
    }
}