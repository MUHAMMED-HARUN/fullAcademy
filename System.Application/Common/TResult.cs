
using Gridify;
using System;
using System.Application.Services.Enrollment.TeachingAssignment.DTO;

namespace System.Application.Common
{
    public class TResult<T> : Result
    {
        public T Value { get; private set; }
        private TResult(bool isSuccess, string error, T value) : base(isSuccess, error)
        {
            Value = value;
        }

        public static TResult<T> OK(T value) => new TResult<T>(true, null, value);
        public static new TResult<T> Fail(string error) => new TResult<T>(false, error, default);
        public static new TResult<T> Fail(Exceptions.ApplicationExceptionObj error) => new TResult<T>(false, error.Message, default);

   

   
    }
}
