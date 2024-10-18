using Business.Entity;
using Business.Interface;
using Business.Repository;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Lab1App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductRepository _productRepository;

        public ProductController(ILogger<ProductController> logger, ProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productRepository.GetAllAsync();
            var productsSimplified = products?.Select(p => new ProductSimplifiedDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            });
            _logger.LogInformation("Get all products");
            return Ok(productsSimplified);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NoContent();
            }
            _logger.LogInformation("Get product with id {id}", id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            try
            {
                var newProduct = await _productRepository.AddAsync(product);
                return CreatedAtAction(nameof(Get), new { id = newProduct.Id }, newProduct);
            }
            catch (AlreadyExistsException e)
            {
                return BadRequest(e.Message);
            }
            catch (BadInputException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Product product)
        {
            try
            {
                await _productRepository.UpdateAsync(product);
                return Ok(product);
            }
            catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (AlreadyExistsException e) // f.e. when trying to update a product with a name that already exists
            {
                return BadRequest(e.Message);
            }
            catch (BadInputException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var product = await _productRepository.DeleteAsync(id);
                return Ok(product);
            } 
            catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }

        }

    }
}
