using API.Model.Classes.Persistence;
using API.Model.Classes.View;
using AutoMapper;
using Structure.Domain.Classes;

namespace API.MappingProfile.Classes
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>();
            CreateMap<ProductPersistenceModel, Product>();

            CreateMap<Campaign, CampaignViewModel>();
            CreateMap<CampaignViewModel, Campaign>();
            CreateMap<CampaignPersistenceModel, Campaign>();
        }
    }
}
