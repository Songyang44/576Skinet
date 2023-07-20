using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Error;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            }
            );

            services.AddScoped<IProductRepository, ProductsRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //AppDomain.CurrentDomain.GetAssemblies()：这是一个方法调用，获取当前应用程序域中加载的所有程序集。
            //GetAssemblies() 方法返回一个 Assembly 数组，其中包含了当前应用程序域中加载的所有程序集。
            //AppDomain. CurrentDomain. GetAssemblys(): This is a method call that retrieves all assemblies loaded in the current application domain. 
            //The GetAssemblys() method returns an Assembly array that contains all the assemblies loaded in the current application domain.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //1.获取模型验证错误信息：通过访问 actionContext.ModelState，筛选出包含错误的模型状态，并将错误消息收集为一个字符串数组 errors。
            //2.创建自定义错误响应：使用 ApiValidationErrorResponse 类创建了一个自定义的错误响应对象 errorResponse，该对象包含了 errors 数组作为错误信息。
            //3.返回 BadRequestObjectResult：使用 new BadRequestObjectResult(errorResponse) 返回一个 BadRequestObjectResult，其中包含了自定义的错误响应。
            //这样，在模型验证失败时，将会返回这个自定义的 BadRequest 响应。
            //1.Obtain model validation error information: By accessing actionContext. ModelState, filter out the model states containing errors, 
            //and collect the error messages into a string array of errors.
            //2.Create custom error response: A custom error response object, errorResponse, 
            //was created using the ApiValidationErrorResponse class, 
            //which contains an array of errors as error information.
            //3.Return BadRequestObject Result: Use new BadRequestObject Result (errorResponse) to return a BadRequestObject Result that contains a custom error response. 
            //In this way, when the model validation fails, the customized BadRequest response will be returned.
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                     var errors = actionContext.ModelState
                     .Where(e => e.Value.Errors.Count > 0)
                     .SelectMany(x => x.Value.Errors)
                     .Select(x => x.ErrorMessage).ToArray();

                     var errorResponse = new ApiValidationErrorResponse
                    {
                         Errors = errors
                     };

                 return new BadRequestObjectResult(errorResponse);
                };


            });
            return services;
        }
    }
}