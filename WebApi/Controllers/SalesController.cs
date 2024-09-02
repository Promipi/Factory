using AutoMapper;
using Common.Collection;
using Common.Core.Contracts.Sales;
using Common.Core.Domain;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet]
        public async Task<ActionResult<DataCollection<Sale>>> GetSales(int page = 1, int take = 10, string customerId = "")
        {
            //filters
            Func<Sale, bool> filter = new Func<Sale, bool>(x => x.Id == x.Id);
            if (customerId != "") filter = new(x => x.CustomerId == customerId);

            var sales = await _context.Sales.Where(filter).GetPagedAsync(page, take);

            return Ok(sales);
        }

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
        /// Crea una nueva venta.
        /// </summary>
        /// <param name="saleCreateDto">Los detalles de la venta a crear.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost]
        public async Task<ActionResult<Sale>> CreateSale([FromBody] SaleCreateDto sale)
        {
            //mapping
            var saleToAdd = _mapper.Map<Sale>(sale);

            //set the total
            foreach(var item in saleToAdd.Items)
            {
                 
                var product = await _context.Products.FindAsync(item.ProductId);
                if(product != null) {
                    item.SubTotal = product.Price * item.Quantity;
                    saleToAdd.Total += product.Price * item.Quantity;
                } 
                else {
                    return BadRequest($"The product with {item.ProductId} doesn't exits ");
                }
                    
            }

            await _context.Sales.AddAsync(saleToAdd);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSale", new { id = saleToAdd.Id }, saleToAdd);
        }

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

            return NoContent();
        }

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

            return NoContent();
        }
    }
}
