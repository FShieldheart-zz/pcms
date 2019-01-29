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

    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public CampaignController(ICampaignService campaignService, IMapper mapper, IMemoryCache memoryCache)
        {
            _campaignService = campaignService;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignViewModel>>> Get([FromQuery]int pageIndex = 0, [FromQuery]int pageSize = 10)
        {
            if (_memoryCache.TryGetValue(Startup.CampaignInMemoryCacheKey, out IEnumerable<Campaign> campaigns))
            {
                campaigns = campaigns.Skip((pageIndex) * pageSize).Take(pageSize);
            }
            else
            {
                campaigns = await _campaignService.GetCampaignsAsync(null, null, true);

                _memoryCache.Set(Startup.CampaignInMemoryCacheKey, campaigns, TimeSpan.FromMinutes(Startup.CacheTimeoutMinute));

                campaigns = campaigns.Skip((pageIndex) * pageSize).Take(pageSize);
            }

            return Ok(_mapper.Map<IEnumerable<CampaignViewModel>>(campaigns));
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> Count()
        {
            int campaignCount;
            if (_memoryCache.TryGetValue(Startup.CampaignInMemoryCacheKey, out IEnumerable<Campaign> campaigns))
            {
                campaignCount = campaigns.Count();
            }
            else
            {
                campaignCount = await _campaignService.CountCampaignsAsync();
            }

            return Ok(campaignCount);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CampaignViewModel>> Get(int id)
        {
            Campaign existedCampaign;
            if (_memoryCache.TryGetValue(Startup.CampaignInMemoryCacheKey, out IEnumerable<Campaign> campaigns))
            {
                existedCampaign = campaigns.FirstOrDefault(p => p.Id.Equals(id));
            }
            else
            {
                existedCampaign = await _campaignService.GetCampaignAsync(id, true);
            }

            return Ok(_mapper.Map<CampaignViewModel>(existedCampaign));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Post([FromBody] CampaignPersistenceModel campaignPersistenceModel)
        {
            if (campaignPersistenceModel == null)
            {
                return BadRequest("Campaign can not be null.");
            }
            else if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrors());
            }

            Campaign campaign = _mapper.Map<Campaign>(campaignPersistenceModel);

            bool insertResult = await _campaignService.AddAsync(campaign);

            _memoryCache.Remove(Startup.CampaignInMemoryCacheKey);

            return Ok(insertResult);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Put(int id, [FromBody] CampaignPersistenceModel campaignPersistenceModel)
        {
            Campaign campaign = await _campaignService.GetCampaignAsync(id);
            campaign = _mapper.Map(campaignPersistenceModel, campaign);

            bool updateResult = await _campaignService.UpdateAsync(campaign);

            _memoryCache.Remove(Startup.CampaignInMemoryCacheKey);

            return Ok(updateResult);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            bool deleteResult = await _campaignService.DeleteAsync(id);

            _memoryCache.Remove(Startup.CampaignInMemoryCacheKey);

            return Ok(deleteResult);
        }
    }
}
