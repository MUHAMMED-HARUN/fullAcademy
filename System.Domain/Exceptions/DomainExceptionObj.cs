
namespace System.Domain.Exceptions
{
    public class DomainExceptionObj
    {
        public string Message { get; private set; }
        public string Code { get; private set; }
        public string FullMessage { get { return Code + " || " + Message; } }
        public DomainExceptionObj(string code, string message)
        {
            Message = message;
            Code = code;
        }
    }
}
