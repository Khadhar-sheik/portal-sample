using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Org.Common.Web.Logging
{
    public class RequestLogModel
    {
        private static readonly string[] PasswordFieldPatterns = { "password", "secret" };
        private string _responseBody;

        public string Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string AppZone { get; set; }
        public string Pod { get; set; }

        public string AuthorizationScheme { get; set; }
        public string AuthorizationValue { get; set; }

        public string Protocol { get; set; }
        public string ClientIp { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public string RequestHeaders { get; set; }
        public string RequestBody { get; set; }

        public string ResponseCode { get; set; }
        public string ResponseHeaders { get; set; }

        public string ResponseBody
        {
            get => _responseBody;
            set => _responseBody = ReplacePasswordValues(value);
        }

        public RequestLogModel(HttpContext context)
        {
            Id = context.Response.Headers[RequestLoggingMiddleware.ResponseIdHeaderName];
            Pod = Environment.GetEnvironmentVariable("MY_POD_NAME");

            AuthenticationHeaderValue.TryParse(context.Request.Headers["Authorization"], out var authorizationValues);
            AuthorizationScheme = authorizationValues?.Scheme;
            AuthorizationValue = authorizationValues?.Parameter;

            Protocol = context.Request.Scheme;
            ClientIp = context.Connection.RemoteIpAddress.ToString();
            Method = context.Request.Method;
            Path = context.Request.Path.ToString();
            QueryString = context.Request.QueryString.ToString();
            RequestHeaders = string.Join(' ', context.Request.Headers?.Select(v => $"{v.Key}:{v.Value.ToString()}"));
            

            ResponseCode = context.Response.StatusCode.ToString();
            ResponseHeaders = string.Join(' ', context.Response.Headers?.Select(v => $"{v.Key}:{v.Value.ToString()}"));
        }

        private string ReplacePasswordValues(string input) => PasswordFieldPatterns
            .Select(pattern => "(\".*?" + pattern + "\":\")[^\"]+")
            .Aggregate(input, (current, matchPattern) => Regex.Replace(current, matchPattern, "$1********", RegexOptions.IgnoreCase));
    }
}
