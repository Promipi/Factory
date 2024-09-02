using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("FactoryPolicy")]
    [Authorize]
    public sealed class CustomersController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CustomersController(ILogger<ProductsController> logger, DataContext dataContext, IMapper mapper)
        {           
            _logger = logger;
            _context = dataContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <remarks>
        /// Requires a valid JWT token to access.
        /// </remarks>
        /// <response code="200">Returns the list of customers.</response>
        /// <response code="401">Unauthorized. Token is missing or invalid.</response>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCustomers()
        {
            var customers = _context.Customers.ToList();
            return Ok(customers);
        }

        /// <summary>
        /// Gets a specific customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <remarks>
        /// Requires a valid JWT token to access.
        /// </remarks>
        /// <response code="200">Returns the customer with the specified ID.</response>
        /// <response code="404">Customer not found.</response>
        /// <response code="401">Unauthorized. Token is missing or invalid.</response>
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        /// <summary>
        /// Deletes a specific customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <remarks>
        /// Requires a valid JWT token to access.
        /// </remarks>
        /// <response code="200">Customer successfully deleted.</response>
        /// <response code="404">Customer not found.</response>
        /// <response code="401">Unauthorized. Token is missing or invalid.</response>
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return Ok();
        }
    }
    
}
