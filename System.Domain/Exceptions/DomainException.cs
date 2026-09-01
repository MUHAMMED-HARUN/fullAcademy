

namespace System.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public string Code { get; private set; }
        public string FullMessage { get { return Code + " || " + Message; } }
        public DomainException(DomainExceptionObj exceptionObj) : base(exceptionObj.Message)
        {
            Code = exceptionObj.Code;
        }
    }
}
