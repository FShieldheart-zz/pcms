using Repository.UnitOfWork.Classes;
using Service.AccessPoint.Interfaces;
using Structure.Domain.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.AccessPoint.Classes
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(IGeneric<Product> productRepository)
        {
            BaseRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(int? page = null, int? pageSize = null)
        {
            if (!page.HasValue)
            {
                page = 0;
            }

            if (!pageSize.HasValue)
            {
                pageSize = 10;
            }

            return await BaseRepository.GetManyAsync(null, p => p.OrderBy(pr => pr.Name), pageSize.Value * page.Value, pageSize.Value);
        }

        public async Task<int> CountProductsAsync()
        {
            return await BaseRepository.CountAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await BaseRepository.GetSingleAsync(id);
        }

        public async Task<bool> AddAsync(Product product)
        {
            return await BaseRepository.InsertAsync(product) == 1 ? true : false;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            return await BaseRepository.UpdateAsync(product) == 1 ? true : false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await BaseRepository.DeleteByIdAsync(id) == 1 ? true : false;
        }

    }
}
