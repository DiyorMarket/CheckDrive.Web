﻿using System.Net;

namespace CheckDrive.Web.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public ApiException(HttpStatusCode statusCode) : base()
        {
            StatusCode = statusCode;
        }

        public ApiException(HttpStatusCode statusCode, string message) :
            base(message)
        {
            StatusCode = statusCode;
        }
    }
}
