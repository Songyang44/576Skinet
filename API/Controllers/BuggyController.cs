using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Error;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController:BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var thing=_context.Product.Find(-100);
            if(thing==null)
            {
                //用于在找不到请求资源时返回一个表示“404 Not Found”的响应。
                //这个方法是ASP.NET Core中的一个内置方法，位于Microsoft.AspNetCore.Mvc命名空间的ControllerBase类中
                //Used to return a response representing '404 Not Found' when the requested resource cannot be found. 
                //This method is a built-in method in the ASP. NET Core, 
                //located in the ControllerBase class of the Microsoft. AspNetCore. Mvc namespace
                return NotFound(new ApiResponse(404));
            }
            return Ok();
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var thing=_context.Product.Find(-100);
            //cannot use ToString to something dosen't exist
            var thisToReturn=thing.ToString();
            return Ok();
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }
    }
}