using Business.Entity;
using Business.Interface;
using Microsoft.EntityFrameworkCore;

namespace Business.Repository
{
    public class ProductRepository : BaseRepository<Product>
    {

        private LogRepository _logRepository;
        public ProductRepository(AppDbContext context, LogRepository logRepository) : base(context)
        {
            _logRepository = logRepository;
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        public override async Task<Product> AddAsync(Product entity)
        {
            // check if same name product already exists
            var existingProduct = await GetByNameAsync(entity.Name);
            if (existingProduct != null)
            {
                throw new AlreadyExistsException($"Product with name {entity.Name} already exists");
            }

            verifyProductOrThrow(entity);

            var newProduct = await base.AddAsync(entity);
            await _logRepository.AddAsync(new Log
            {
                LogMessage = $"Product {newProduct.Name} was added",
                LogTimeUTC = DateTime.Now.ToUniversalTime()
            });
            return newProduct;
        }

        public override async Task<Product> UpdateAsync(Product entity)
        {
            var product = await base.GetByIdAsync(entity.Id);

            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            var productWithSameName = await GetByNameAsync(entity.Name);

            if (productWithSameName != null && productWithSameName.Id != entity.Id)
            {
                throw new AlreadyExistsException($"Product with name {entity.Name} already exists");
            }

            verifyProductOrThrow(entity);

            product.Name = entity.Name;
            product.Description = entity.Description;
            product.Price = entity.Price;
            product.Stock = entity.Stock;

            await base.UpdateAsync(product);
            await _logRepository.AddAsync(new Log
            {
                LogMessage = $"Product {product.Name} was updated",
                LogTimeUTC = DateTime.Now.ToUniversalTime()
            });
            return product;
        }

        public async Task<Product> DeleteAsync(Guid id)
        {
            var product = await base.GetByIdAsync(id);

            if (product == null)
            {
                throw new NotFoundException("Product not found");
            }

            var deletedProduct = await base.DeleteAsync(product);
            await _logRepository.AddAsync(new Log
            {
                LogMessage = $"Product {deletedProduct.Name} was deleted",
                LogTimeUTC = DateTime.Now.ToUniversalTime()
            });
            return deletedProduct;
        }

        private static void verifyProductOrThrow(Product entity)
        {
            if (entity.Name.Length > 20)
            {
                throw new BadInputException("Name cannot be longer than 20 characters");
            }

            if (entity.Name.Length < 3)
            {
                throw new BadInputException("Name cannot be shorter than 3 characters");
            }

            if (entity.Price < 0.01)
            {
                throw new BadInputException("Price must be greater than 0.01");
            }

            if (entity.Stock < 0)
            {
                throw new BadInputException("Stock must be greater than 0");
            }
        }
    }
}
