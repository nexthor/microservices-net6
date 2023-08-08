using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService) => _productService = productService;

        public async Task<IActionResult> Index()
        {
            var products = new List<ProductDto>();
            var response = await _productService.GetAllProductsAsync<ResponseDto>();

            if (response != null && response.IsSuccess)
                products = JsonSerializer.Deserialize<List<ProductDto>>(Convert.ToString(response.Result)!, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

            return View(products);
        }

        public IActionResult CreateProduct() { 
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto form)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProductAsync<ResponseDto>(form);

                if (response != null && response.IsSuccess)
                    return RedirectToAction(nameof(Index));
            }

            return View(form);
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var response = await _productService.GetProductByIdAsync<ResponseDto>(id);
            if (response != null && response.IsSuccess)
            {
                var model = JsonSerializer.Deserialize<ProductDto>(Convert.ToString(response.Result)!, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductDto form)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateProductAsync<ResponseDto>(form);

                if (response != null && response.IsSuccess)
                    return RedirectToAction(nameof(Index));
            }

            return View(form);
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _productService.GetProductByIdAsync<ResponseDto>(id);
            if (response != null && response.IsSuccess)
            {
                var model = JsonSerializer.Deserialize<ProductDto>(Convert.ToString(response.Result)!, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });

                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductDto form)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.DeleteProductAsync<ResponseDto>(form.ProductId);

                if (response != null && response.IsSuccess)
                    return RedirectToAction(nameof(Index));
            }

            return View(form);
        }
    }
}
