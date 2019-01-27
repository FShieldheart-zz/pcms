using API.MappingProfile.Classes;
using API.Model.Classes.Persistence;
using AutoMapper;
using NUnit.Framework;
using Structure.Domain.Classes;
using System;

namespace Tests
{
    public class Tests
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainProfile());
            });

            _mapper = mappingConfig.CreateMapper();
        }

        [Test]
        public void MappingProductToProductModelTest()
        {
            DateTime mockCreatedDate = new DateTime(2019, 1, 1);
            DateTime mockUpdateDate = new DateTime(2020, 1, 1);
            int mockId = 12;
            bool mockIsDeleted = false;
            string mockName = "Trial";
            string mockNewName = "Error";

            Product mockProduct = new Product()
            {
                CreatedDate = mockCreatedDate,
                UpdatedDate = mockUpdateDate,
                Id = mockId,
                IsDeleted = mockIsDeleted,
                Name = mockName
            };

            ProductPersistenceModel mockProductModel = new ProductPersistenceModel()
            {
                Name = mockNewName
            };

            mockProduct = _mapper.Map(mockProductModel, mockProduct);

            Assert.AreEqual(mockProduct.Name, mockNewName);

            Assert.AreEqual(mockProduct.CreatedDate, mockCreatedDate);
            Assert.AreEqual(mockProduct.UpdatedDate, mockUpdateDate);
            Assert.AreEqual(mockProduct.Id, mockId);
            Assert.AreEqual(mockProduct.IsDeleted, mockIsDeleted);

            Assert.Pass();
        }
    }
}