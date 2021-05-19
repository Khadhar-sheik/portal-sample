using System;
using System.Collections.Generic;
using System.Text;

namespace Org.Common.Web.Options
{
    public class CorsOptions
    {
        public bool Enabled { get; set; }
        public string AllowedHosts { get; set; }
    }
}
