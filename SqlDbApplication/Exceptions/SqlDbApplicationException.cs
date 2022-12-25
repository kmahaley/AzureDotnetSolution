using System;

namespace SqlDbApplication.Exceptions
{
    public class SqlDbApplicationException : Exception
    {
        public ErrorCode ErrorCode { get; private set; }

        public SqlDbApplicationException(string message) : base(message) { }
        public SqlDbApplicationException(string message, Exception inner) : base(message, inner) { }

        public SqlDbApplicationException(string message, ErrorCode errorCode) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public SqlDbApplicationException(
            string message,
            ErrorCode errorCode,
            Exception inner) : base(message, inner) 
        {
            this.ErrorCode = errorCode;
        }


    }

}
