using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
   
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;

        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")] // Get : buggy/notfound
        public ActionResult GetNotFoundRequest()
        {
            var product = _context.Products.Find(100);
            if(product == null)
                return NotFound(new ApiResponse(404));
            return Ok(product);
        }

        [HttpGet("servererror")] // Get : buggy/servererror
        public ActionResult GetServerError()
        {
            var product = _context.Products.Find(100);
            var productToReturn = product.ToString(); // will through exception
            return Ok(productToReturn);
        }

        [HttpGet("badrequest")] // Get : buggy/badrequest
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }


        [HttpGet("badrequest/{id}")] // Get : buggy/badrequest/five
        public ActionResult GetBadRequest(int id) // validation error 
        {
            return Ok();
        }

    }
}
