using AutoMapper;
using Common.Collection;
using Common.Core.Contracts.Sales;
using Common.Core.Domain;
using Infrastructure.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("FactoryPolicy")]
    public class SalesController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SalesController(ILogger<ProductsController> logger, DataContext dataContext, IMapper mapper)
        {
            _logger = logger;
            _context = dataContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of sales with optional filtering by customer ID.
        /// </summary>
        /// <param name="page">The page number to retrieve.</param>
        /// <param name="take">The number of records per page.</param>
        /// <param name="customerId">An optional customer ID to filter sales by customer.</param>
        /// <returns>A paginated list of sales.</returns>
        /// <response code="200">Returns the paginated list of sales.</response>
        /// <response code="400">Returns a Bad Request if the request parameters are invalid.</response>
        [HttpGet]
        public async Task<ActionResult<DataCollection<Sale>>> GetSales(int page = 1, int take = 10, string customerId = "")
        {
            //filters
            Func<Sale, bool> filter = new Func<Sale, bool>(x => x.Id == x.Id);
            if (customerId != "") filter = new(x => x.CustomerId == customerId);

            var sales = await _context.Sales.Where(filter).GetPagedAsync(page, take);

            return Ok(sales);
        }

        /// <summary>
        /// Retrieves a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to retrieve.</param>
        /// <returns>The sale with the specified ID.</returns>
        /// <response code="200">Returns the sale with the specified ID.</response>
        /// <response code="404">Returns Not Found if the sale with the specified ID does not exist.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(string id)
        {
            var sale = await _context.Sales.FindAsync(id);

            if (sale == null)
            {
                return NotFound();
            }

            return Ok(sale);
        }

        /// <summary>
        /// Creates a new sale.
        /// </summary>
        /// <param name="sale">The sale data to create.</param>
        /// <returns>The created sale.</returns>
        /// <response code="201">Returns the created sale with the location of the newly created resource.</response>
        /// <response code="400">Returns Bad Request if the product specified in the sale does not exist.</response>
        [HttpPost]
        public async Task<ActionResult<Sale>> CreateSale([FromBody] SaleCreateDto sale)
        {
            //mapping
            var saleToAdd = _mapper.Map<Sale>(sale);

            //set the total
            foreach (var item in saleToAdd.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    item.SubTotal = product.Price * item.Quantity;
                    saleToAdd.Total += product.Price * item.Quantity;
                }
                else
                {
                    return BadRequest($"The product with {item.ProductId} doesn't exist.");
                }
            }

            var saleSaved = await _context.Sales.AddAsync(saleToAdd);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSale", new { id = saleToAdd.Id }, saleToAdd);
        }

        /// <summary>
        /// Updates an existing sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to update.</param>
        /// <param name="sale">The updated sale data.</param>
        /// <returns>An action result indicating the outcome of the update operation.</returns>
        /// <response code="200">Returns Ok if the sale was successfully updated.</response>
        /// <response code="404">Returns Not Found if the sale with the specified ID does not exist.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSale(string id, SaleUpdateDto sale)
        {
            var saleToUpdate = await _context.Sales.FindAsync(id);

            if (saleToUpdate == null)
            {
                return NotFound();
            }

            //mapping
            _mapper.Map(sale, saleToUpdate);

            await _context.SaveChangesAsync();

            return Ok("Sale Updated");
        }

        /// <summary>
        /// Deletes a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to delete.</param>
        /// <returns>An action result indicating the outcome of the delete operation.</returns>
        /// <response code="200">Returns Ok if the sale was successfully deleted.</response>
        /// <response code="404">Returns Not Found if the sale with the specified ID does not exist.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(string id)
        {
            var sale = await _context.Sales.FindAsync(id);

            if (sale == null)
            {
                return NotFound();
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();

            return Ok("Sale Deleted");
        }
    }
}
