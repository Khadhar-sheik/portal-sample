using System;
using System.Collections.Generic;
using System.Text;

namespace Org.Common.Web.Logging
{
    public class LoggingOptions
    {
        public string AppZone { get; set; }
        public string ElasticSearchUri { get; set; }
        public string ErrorLoggingIndex { get; set; }
        public string RequestLoggingIndex { get; set; }
        public bool RequestLoggingEnabled { get; set; }

        public LoggingOptions()
        {
            RequestLoggingEnabled = true;
        }
    }
}
