using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
            Console.WriteLine(typeof(DataContext).Assembly.FullName);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new List<string>
            {
                "1", "2" , "3"
            };
        }
        
    }
}
