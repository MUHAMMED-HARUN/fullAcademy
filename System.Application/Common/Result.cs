
using System;

namespace System.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result OK() => new Result(true, null);
        public static Result Fail(string error) => new Result(false, error);
        public static Result Fail(Exceptions.ApplicationExceptionObj error) => new Result(false, error.Message);
    }
}
