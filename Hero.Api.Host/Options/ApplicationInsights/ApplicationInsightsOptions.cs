using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hero.Api.Host.Options.ApplicationInsights
{
    public class ApplicationInsightsOptions
    {
        public const string Section = "ApplicationInsights";
        public bool IsEnabled { get; set; } = false;
    }
}
