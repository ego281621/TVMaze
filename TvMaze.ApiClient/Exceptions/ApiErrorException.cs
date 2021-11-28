
using System;
using System.Net;

namespace TvMaze.ApiClient
{
    public class ApiErrorException: Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApiErrorException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
