using AutoMapper;
using Common.Collection;
using Common.Core.Contracts.Products;
using Common.Core.Domain;
using Infrastructure.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("FactoryPolicy")]
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

        /// <summary>
        /// Retrieves a list of products with pagination and optional filters.
        /// </summary>
        /// <param name="page">Page number for pagination.</param>
        /// <param name="take">Number of items per page.</param>
        /// <param name="contains">String to filter products by name.</param>
        /// <param name="category">Category to filter products by.</param>
        /// <returns>A collection of filtered and paginated products.</returns>
        [ProducesResponseType(typeof(DataCollection<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<ActionResult<DataCollection<Product>>> GetProducts(int page=1,int take=10,string contains="",string category="")
        {
            //filters
            Func<Product, bool> filter = new Func<Product, bool>(x => x.Id == x.Id);
            if(contains!="") filter = new (x => x.Name.ToLowerInvariant().Contains(contains.ToLowerInvariant()));

            var products = await _context.Products.Where(filter).GetPagedAsync(page,take);

            return Ok(products);
        }
        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <returns>The product corresponding to the specified ID.</returns>
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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


        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">Product data to create.</param>
        /// <returns>The created product.</returns>
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ProductCreateDto product)
        {
            //mapping
            var productToAdd = _mapper.Map<Product>(product);

            await _context.Products.AddAsync(productToAdd);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = productToAdd.Id }, productToAdd);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="product">Product data to update.</param>
        /// <returns>The updated product.</returns>
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">Product ID.</param>
        /// <returns>A status message indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
