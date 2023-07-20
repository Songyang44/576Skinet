using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Error;

namespace API.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env
        )
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        //ASP.NET Core 中的 middleware，它用于捕获应用程序中的异常，并返回适当的响应。

        //`InvokeAsync` 方法是 middleware 必须实现的方法之一，用于处理 HTTP 请求。在该方法中，首先调用 `_next(context)` 将请求传递给管道中的下一个中间件或终止程序，如果在此期间没有引发异常，则请求将按预期执行。

        //如果在此过程中引发了异常，则在 `catch` 块中捕获该异常。在这个块中，将记录异常信息，并设置一个适当的 HTTP 响应以通知客户端出现了错误。响应状态码被设置为 `500 Internal Server Error`，并且响应主体包含有关错误的详细信息。响应主体的内容由 `_env.IsDevelopment()` 的值来决定，如果当前环境是开发环境，则返回包含详细错误信息的 `ApiException` 对象；否则，只返回一个空的 `ApiException` 对象。

        //最后，将响应主体序列化为 JSON 格式，并写入到 HTTP 响应中，以便客户端可以读取和处理错误信息。这是通过使用 `JsonSerializer` 类来完成的，该类将对象序列化为 JSON 字符串，然后通过 `await context.Response.WriteAsync(json)` 将其写入到 HTTP 响应主体中。
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //if there's no exception, then the reqest moves on to its next stage
                await _next(context);
            }
            catch(Exception ex)
            {
                //使用 _logger 对象调用 LogError 方法，并传递两个参数：第一个参数是异常对象 ex，第二个参数是异常的消息 ex.Message。
                //Using _logger object calls the LogError method and passes two parameters: 
                //the first parameter is the exception object ex, 
                //and the second parameter is the exception message ex.Message.
                _logger.LogError(ex,ex.Message);
                //设置HTTP响应的Content-Type标头为"application/json"。这告诉客户端收到的数据是JSON格式的。
                //Set the Content-Type header of the HTTP response to 'application/JSON'. 
                //This tells the client that the data received is in JSON format.
                context.Response.ContentType="application/json";
                //设置HTTP响应的状态码为500，表示"Internal Server Error"，即服务器端发生了错误。
                //HttpStatusCode.InternalServerError 是一个枚举值，代表HTTP状态码500。
                //Set the status code of the HTTP response to 500, indicating "Internal Server Error", 
                //meaning that an error occurred on the server side. HttpStatusCode. 
                //InternalServerError is an enumeration value representing the List of HTTP status codes 500.
                context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
                //表示当前应用程序的环境,如果应用程序运行在开发环境下，将包含异常的消息和堆栈信息，否则仅创建一个状态码为500的 ApiException 对象。
                //Represents the current application environment. If the application is running in a development environment, it will contain abnormal messages and stack information. 
                //Otherwise, only an ApiException object with a status code of 500 will be created.
                var response=_env.IsDevelopment()
                ?new ApiException((int)HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace.ToString())
                :new ApiException((int)HttpStatusCode.InternalServerError);

                //指定在进行 JSON 序列化时，属性名称应该采用驼峰命名法。
                //When specifying JSON serialization, attribute names should use the hump naming convention.
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                //调用 JsonSerializer.Serialize 方法后，将返回一个表示序列化后 JSON 数据的字符串，
                //并将其赋值给 json 变量。可以将 response 对象序列化为符合配置的 JSON 格式。
                //After calling the JsonSerializer. 
                //Serialize method, a string representing the serialized JSON data will be returned and assigned to the JSON variable. 
                //The response object can be serialized into a JSON format that conforms to the configuration.
                var json=JsonSerializer.Serialize(response,options);

                //ASP.NET Core中的HTTP响应对象，它用于向客户端发送HTTP响应。通过 WriteAsync 方法，您可以将内容异步写入响应的主体。
                //The HTTP response object in the ASP. NET Core, which is used to send HTTP responses to clients. 
                //Through the WriteAsync method, you can asynchronously write content to the body of the response.
                await context.Response.WriteAsync(json);

            }
        }
    }
}