using Structure.Domain.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.AccessPoint.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(int? page = null, int? pageSize = null);
        Task<IEnumerable<Product>> GetProductsAsync(IEnumerable<int> productIds);
        Task<Product> GetProductAsync(int id);
        Task<bool> AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<int> CountProductsAsync();
        Task<IEnumerable<Product>> SearchProductsAsync(string searchKeyword);
    }
}