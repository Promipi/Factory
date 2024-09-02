using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    [EnableCors("FactoryPolicy")]
    [Authorize]
    public sealed class CustomersController : ControllerBase
    {
        public CustomersController()
        {

        }

        [HttpGet("GetCustomers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCustomers()
        {
            return Ok("Customers");
        }
    }
    
}
