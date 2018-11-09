using Dto = AmpedBiz.Service.Dto;


namespace AmpedBiz.Tests.UnitTests
{
	public static class MockData
    {
        public static class Dtos
        {
            public static Dto.Address CreateAddress()
            {
                return new Service.Dto.Address()
                {
                    Street = "Dto Street",
                    Barangay = "Dto Barangay",
                    City = "Dto City",
                    Province = "Dto Province",
                    Region = "Dto Region",
                    Country = "Dto Country",
                    ZipCode = "Dto Zipcode"
                };
            }
        }

        public static class Entities
        {
            public static Core.Common.Address CreateAddress()
            {
                return new Core.Common.Address()
                {
                    Street = "Entity Street",
                    Barangay = "Entity Barangay",
                    City = "Entity City",
                    Province = "Entity Province",
                    Region = "Entity Region",
                    Country = "Entity Country",
                    ZipCode = "Entity Zipcode"
                };
            }
        }
    }

}
