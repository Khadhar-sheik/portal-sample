namespace Org.Common.Exception
{
    public abstract class OperationException : System.Exception
    {
        private const string STATUS_CODE_KEY = "StatusCode";

        public int? StatusCode
        {
            get => (int) Data[STATUS_CODE_KEY];
            set => Data[STATUS_CODE_KEY] = value;
        }

        protected OperationException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
