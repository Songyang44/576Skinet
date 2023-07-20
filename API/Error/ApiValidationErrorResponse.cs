using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Error
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {

        }
        //`Errors` 属性是一个 `IEnumerable<string>` 类型的公共属性，用于保存错误消息的集合。
        //在 API 中使用这个类时，可以将所有验证错误消息存储在这个属性中，并将 `ApiValidationErrorResponse` 对象作为响应的一部分返回给客户端。
        //`The Errors attribute is a public attribute of type 'IEnumerable<string>' used to store a collection of error messages. 
        //When using this class in the API, 
        //all validation error messages can be stored in this property and the 'ApiValidationErrorResponse' object can be returned to the client as part of the response.
        public IEnumerable<string> Errors { get; set; }
        
        
    }
}