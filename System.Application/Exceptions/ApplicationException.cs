
using System;

namespace System.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationExceptionObj ErrorInfo { get; private set; }
        public string FullMessage => ErrorInfo.FullMessage;

        public ApplicationException(ApplicationExceptionObj errorInfo) : base(errorInfo.Message)
        {
            ErrorInfo = errorInfo;
        }
    }
}
