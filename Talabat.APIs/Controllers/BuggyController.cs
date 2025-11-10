using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    public class BuggyController : ApiBaseController
    {
        private readonly StoreContext context;

        public BuggyController(StoreContext _context)
        {
            context = _context;
        }


        #region NotFound Error
        [HttpGet("notfound")] // GET : api/buggy/notfound
        public ActionResult GetNotFoundRequest()
        {
            var product = context.Products.Find(100); // Enter value for number not available

            if (product is null)
                return NotFound(new ApiResponse(404));

            return Ok(product);
        }
        #endregion


        #region Server Error
        [HttpGet("servererror")] // GET : api/buggy/servererror
        public ActionResult GetServerError()
        {
            var product = context.Products.Find(100);

            var mappedProduct = product.ToString(); // Will Throw Exception [Null Reference Exception]

            return Ok(mappedProduct);
        }
        #endregion


        #region BadRequest Error
        [HttpGet("badrequest")] // GET : api/buggy/badrequest
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        #endregion


        #region Validation Error
        [HttpGet("badrequest/{id}")] // GET : api/buggy/badrequest/five   (string)
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }
        #endregion
    }
}
