using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IProductService
    {
        Task<T> GetAllProductsAsync<T>();
        Task<T> GetProductByIdAsync<T>(int id);
        Task<T> CreateProductAsync<T>(ProductDto dto);
        Task<T> UpdateProductAsync<T>(ProductDto dto);
        Task<T> DeleteProductAsync<T>(int id);
    }
}
