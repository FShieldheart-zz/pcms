using API.Model.Classes;
using AutoMapper;
using Structure.Domain.Classes;

namespace API.MappingProfile.Classes
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();
        }
    }
}
