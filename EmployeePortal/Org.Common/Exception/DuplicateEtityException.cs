using System.Net;

namespace Org.Common.Exception
{
    public class DuplicateEtityException : OperationException
    {
        public DuplicateEtityException(string message) : base(message, (int)HttpStatusCode.Conflict)
        {
        }
    }
}
