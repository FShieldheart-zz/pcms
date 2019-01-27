using System.Collections.Generic;
using System.Threading.Tasks;
using Structure.Domain.Classes;

namespace Service.AccessPoint.Interfaces
{
    public interface ICampaignService
    {
        Task<bool> AddAsync(Campaign campaign);
        Task<int> CountCampaignsAsync();
        Task<bool> DeleteAsync(int id);
        Task<Campaign> GetCampaignAsync(int id);
        Task<IEnumerable<Campaign>> GetCampaignsAsync(int? page = null, int? pageSize = null);
        Task<bool> UpdateAsync(Campaign campaign);
    }
}