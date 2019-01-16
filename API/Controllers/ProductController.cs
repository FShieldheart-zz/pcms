using API.Helper.Classes;
using API.Model.Classes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Service.AccessPoint.Interfaces;
using Structure.Domain.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public ProductController(IProductService productService, IMapper mapper, IMemoryCache memoryCache)
        {
            _productService = productService;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Get([FromQuery]int pageIndex = 0, [FromQuery]int pageSize = 10)
        {
            if (_memoryCache.TryGetValue(Startup.InMemoryCacheKey, out IEnumerable<Product> products))
            {
                return Ok(products.Skip((pageIndex) * pageSize).Take(pageSize));
            }

            products = await _productService.GetProductsAsync();

            _memoryCache.Set(Startup.InMemoryCacheKey, products, TimeSpan.FromMinutes(30));

            return Ok(products.Skip((pageIndex) * pageSize).Take(pageSize));
        }

        [HttpGet("count")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> Count()
        {
            if (_memoryCache.TryGetValue(Startup.InMemoryCacheKey, out IEnumerable<Product> products))
            {
                return Ok(products.Count());
            }

            int productCount = (await _productService.CountProductsAsync());

            _memoryCache.Set(Startup.InMemoryCacheKey, products, TimeSpan.FromMinutes(30));

            return Ok(productCount);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            if (_memoryCache.TryGetValue(Startup.InMemoryCacheKey, out IEnumerable<Product> products))
            {
                return Ok(products.FirstOrDefault(p => p.Id.Equals(id)));
            }

            return Ok(await _productService.GetProductAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductModel productModel)
        {
            if (productModel == null)
            {
                return BadRequest("User can not be null.");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrors());
            }

            Product product = _mapper.Map<Product>(productModel);

            bool insertResult = await _productService.AddAsync(product);

            _memoryCache.Remove(Startup.InMemoryCacheKey);

            return Ok(insertResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductModel productModel)
        {
            Product product = await _productService.GetProductAsync(id);
            product = _mapper.Map(productModel, product);

            bool updateResult = await _productService.UpdateAsync(product);

            _memoryCache.Remove(Startup.InMemoryCacheKey);

            return Ok(updateResult);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleteResult = await _productService.DeleteAsync(id);

            _memoryCache.Remove(Startup.InMemoryCacheKey);

            return Ok(deleteResult);
        }
    }
}
