using AutoMapper;
using Common.Collection;
using Common.Core.Contracts.Products;
using Common.Core.Domain;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductsController(ILogger<ProductsController> logger , DataContext dataContext , IMapper mapper)
        {
            _logger = logger;
            _context = dataContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<DataCollection<Product>>> GetProducts(int page=1,int take=10,string contains="")
        {
            //filters
            Func<Product, bool> filter = new Func<Product, bool>(x => x.Id == x.Id);
            if(contains!="") filter = new (x => x.Name.ToLowerInvariant().Contains(contains.ToLowerInvariant()));

            var products = await _context.Products.Where(filter).GetPagedAsync(page,take);

            return Ok(products);
        }

        [HttpGet("{id}", Name = "")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ProductCreateDto product)
        {
            //mapping
            var productToAdd = _mapper.Map<Product>(product);

            await _context.Products.AddAsync(productToAdd);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = productToAdd.Id }, productToAdd);
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(ProductUpdateDto product)
        {
            var productToUpdate = await _context.Products.FindAsync(product.Id);

            if(productToUpdate == null)
            {
                return NotFound();
            }

            //mapping
            _mapper.Map(product, productToUpdate);

            _context.Products.Update(productToUpdate);
            await _context.SaveChangesAsync();

            return Ok(productToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            var product = await _context.Products.FindAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok("Product Deleted");
        }
    }
}
