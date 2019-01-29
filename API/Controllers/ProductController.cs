using API.Helper.Classes;
using API.Model.Classes.Persistence;
using API.Model.Classes.View;
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
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> Get([FromQuery]int pageIndex = 0, [FromQuery]int pageSize = 10)
        {
            if (_memoryCache.TryGetValue(Startup.ProductInMemoryCacheKey, out IEnumerable<Product> products))
            {
                products = products.Skip((pageIndex) * pageSize).Take(pageSize);
            }
            else
            {
                products = await _productService.GetProductsAsync();

                _memoryCache.Set(Startup.ProductInMemoryCacheKey, products, TimeSpan.FromMinutes(Startup.CacheTimeoutMinute));

                products = products.Skip((pageIndex) * pageSize).Take(pageSize);
            }

            return Ok(_mapper.Map<IEnumerable<ProductViewModel>>(products));
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            int productCount;
            if (_memoryCache.TryGetValue(Startup.ProductInMemoryCacheKey, out IEnumerable<Product> products))
            {
                productCount = products.Count();
            }
            else
            {
                productCount = await _productService.CountProductsAsync();
            }

            return Ok(productCount);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> Search([FromQuery]string searchKeyword)
        {
            if (string.IsNullOrWhiteSpace(searchKeyword))
            {
                return BadRequest("Product Search Keyword can not be empty or null.");
            }

            IEnumerable<Product> seachedProducts;

            if (_memoryCache.TryGetValue(Startup.ProductInMemoryCacheKey, out IEnumerable<Product> products))
            {
                seachedProducts = products.Where(p => p.Name.ToLowerInvariant().Contains(searchKeyword.ToLowerInvariant()))
                    .OrderBy(p => p.Name);
            }
            else
            {
                seachedProducts = await _productService.SearchProductsAsync(searchKeyword);
            }

            return Ok(_mapper.Map<IEnumerable<ProductViewModel>>(seachedProducts));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductViewModel>> Get(int id)
        {
            Product existedProduct;
            if (_memoryCache.TryGetValue(Startup.ProductInMemoryCacheKey, out IEnumerable<Product> products))
            {
                existedProduct = products.FirstOrDefault(p => p.Id.Equals(id));
            }
            else
            {
                existedProduct = await _productService.GetProductAsync(id);
            }

            return Ok(_mapper.Map<ProductViewModel>(existedProduct));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody] ProductPersistenceModel productPersistenceModel)
        {
            if (productPersistenceModel == null)
            {
                return BadRequest("Product can not be null.");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrors());
            }

            Product product = _mapper.Map<Product>(productPersistenceModel);

            bool insertResult = await _productService.AddAsync(product);

            _memoryCache.Remove(Startup.ProductInMemoryCacheKey);

            return Ok(insertResult);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(int id, [FromBody] ProductPersistenceModel productPersistenceModel)
        {
            Product product = await _productService.GetProductAsync(id);
            product = _mapper.Map(productPersistenceModel, product);

            bool updateResult = await _productService.UpdateAsync(product);

            _memoryCache.Remove(Startup.ProductInMemoryCacheKey);

            return Ok(updateResult);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            bool deleteResult = await _productService.DeleteAsync(id);

            _memoryCache.Remove(Startup.ProductInMemoryCacheKey);

            return Ok(deleteResult);
        }
    }
}
