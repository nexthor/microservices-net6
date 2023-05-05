using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;
        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDto> CreateUpdateProduct(ProductDto request)
        {
            var product = new Product();

            _mapper.Map(request, product);

            if (product.ProductId > 0)
                _context.Products.Update(product);
            else
                _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDto>(request);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.Where(x => x.ProductId.Equals(id)).FirstOrDefaultAsync();
                if (product == null)
                    return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _context.Products.Where(x => x.ProductId.Equals(id)).FirstOrDefaultAsync();

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var products = await _context.Products.ToListAsync(); ;
            var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            return dtos;
        }
    }
}
