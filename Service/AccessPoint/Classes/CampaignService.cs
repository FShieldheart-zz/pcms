using Repository.UnitOfWork.Classes;
using Service.AccessPoint.Interfaces;
using Structure.Domain.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.AccessPoint.Classes
{
    public class CampaignService : BaseService<Campaign>, ICampaignService
    {
        private readonly IProductService _productService;

        public CampaignService(IGeneric<Campaign> campaignRepository, IProductService productService)
        {
            BaseRepository = campaignRepository;
            _productService = productService;
        }

        public async Task<IEnumerable<Campaign>> GetCampaignsAsync(int? page = null, int? pageSize = null)
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

        public async Task<int> CountCampaignsAsync()
        {
            return await BaseRepository.CountAsync();
        }

        public async Task<Campaign> GetCampaignAsync(int id)
        {
            return await BaseRepository.GetSingleAsync(id);
        }

        public async Task<bool> AddAsync(Campaign campaign)
        {
            if (!campaign.ProductId.HasValue)
            {
                throw new Exception("Product of a campaign can not be null");
            }
            Product campaignProduct = await _productService.GetProductAsync(campaign.ProductId.Value);
            if (campaignProduct == null)
            {
                throw new Exception("Product of a campaign can not be null");
            }
            return await BaseRepository.InsertAsync(campaign) == 1 ? true : false;
        }

        public async Task<bool> UpdateAsync(Campaign campaign)
        {
            if (!campaign.ProductId.HasValue)
            {
                throw new Exception("Product of a campaign can not be null");
            }
            Product campaignProduct = await _productService.GetProductAsync(campaign.ProductId.Value);
            if (campaignProduct == null)
            {
                throw new Exception("Product of a campaign can not be null");
            }
            return await BaseRepository.UpdateAsync(campaign) == 1 ? true : false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await BaseRepository.DeleteByIdAsync(id) == 1 ? true : false;
        }

    }
}
