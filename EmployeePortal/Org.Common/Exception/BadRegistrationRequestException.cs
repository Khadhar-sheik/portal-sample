using System.Net;

namespace Org.Common.Exception
{
    public class BadRegistrationRequestException : OperationException
    {
        public object RegistrationRequestError { get; }
        
        public BadRegistrationRequestException(object registrationRequestError) : base(string.Empty, (int)HttpStatusCode.BadRequest)
        {
            RegistrationRequestError = registrationRequestError;
        }
    }
}