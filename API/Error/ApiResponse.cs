using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Error
{
    public class ApiResponse
    {
         public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have mad!!!",
                401 => "Authorized, you are not",
                404=> "Resource found, it was not",
                500 => "Errors are the path to the dark side. Error lead to Anger. Anger leads to Hate. Hate leads to life change",
                _ => null
            };
        }
    }
}