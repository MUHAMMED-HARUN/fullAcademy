
using System;
using System.Domain.Exceptions;

namespace System.Application.Exceptions
{
    public class ApplicationExceptionObj
    {
        public string Code { get; private set; }
        public string Message { get; private set; }
        public string Source { get; private set; } // Domain | Application | Infrastructure
        public DomainExceptionObj? InnerError { get; private set; }
        public string FullMessage => Message;

        public ApplicationExceptionObj(string code, string message, string source = "Application", DomainExceptionObj? innerError = null)
        {
            Code = code;
            Message = message;
            Source = source;
            InnerError = innerError;
        }

        // Helper to clone/create new with inner error
        public ApplicationExceptionObj WithInnerError(DomainExceptionObj innerError)
        {
             return new ApplicationExceptionObj(Code, Message, Source, innerError);
        }
    }
}
