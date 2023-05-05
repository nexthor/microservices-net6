using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        protected ResponseDto _response;
        private IProductRepository _repository;
        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
            this._response = new ResponseDto();
        }
    }
}
