using AmpedBiz.Common.Extentions;
using NUnit.Framework;
using System;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Tests.UnitTests
{
    [TestFixture]
    public class MappingTest
    {
        [Test]
        public void ShouldMapAddress()
        {
            AssertMapping(
                createDto: () =>
                {
                    return new Dto.Address()
                    {
                        Street = "Dto Street",
                        Barangay = "Dto Barangay",
                        City = "Dto City",
                        Province = "Dto Province",
                        Region = "Dto Region",
                        Country = "Dto Country",
                        ZipCode = "Dto Zipcode"
                    };

                }, 
                createEntity: () =>
                {
                    return new Entity.Address()
                    {
                        Street = "Entity Street",
                        Barangay = "Entity Barangay",
                        City = "Entity City",
                        Province = "Entity Province",
                        Region = "Entity Region",
                        Country = "Entity Country",
                        ZipCode = "Entity Zipcode"
                    };
                }, 
                assertEquality: (value1, value2) =>
                {
                    Assert.AreEqual(value1.Street, value2.Street);
                    Assert.AreEqual(value1.Barangay, value2.Barangay);
                    Assert.AreEqual(value1.City, value2.City);
                    Assert.AreEqual(value1.Province, value2.Province);
                    Assert.AreEqual(value1.Region, value2.Region);
                    Assert.AreEqual(value1.Country, value2.Country);
                    Assert.AreEqual(value1.ZipCode, value2.ZipCode);
                }
            );
        }

        private void AssertMapping<TDto, TEntity>(
            Func<TDto> createDto, 
            Func<TEntity> createEntity, 
            Action<TDto, TEntity> assertEquality
        ) 
            where TDto : class
            where TEntity : class
        {
            var dto = default(TDto);
            var entity = default(TEntity);

            dto = createDto();
            entity = dto.MapTo(default(TEntity));
            assertEquality(dto, entity);

            dto = createDto();
            entity = Activator.CreateInstance<TEntity>();
            dto.MapTo(entity);
            assertEquality(dto, entity);

            entity = createEntity();
            dto = entity.MapTo(default(TDto));
            assertEquality(dto, entity);

            entity = createEntity();
            dto = Activator.CreateInstance<TDto>();
            entity.MapTo(dto);
            assertEquality(dto, entity);
        }
    }
}
