using System.Net;

namespace Org.Common.Exception
{
    public class NotAuthorizedException : OperationException
    {
        public NotAuthorizedException(string message) : base(message, (int)HttpStatusCode.Unauthorized)
        {
        }
    }
}