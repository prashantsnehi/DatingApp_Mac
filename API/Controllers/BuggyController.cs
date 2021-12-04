using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret() => "secret text";
        
        [HttpGet("not-found")]
        public ActionResult<string> GetNotFount() 
        {
            var thing = _context.Users.Find(-1);

            if(thing == null) return NotFound();

            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult GetServerError() {
            var thing = _context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return Ok(thing);
        }
        
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest() => BadRequest("This is bad-request");
    }
}