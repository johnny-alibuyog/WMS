using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Tests.IntegrationTests
{
    internal class DummyData
    {
        private readonly Random random = new Random();

        public HashSet<int> GenerateUniqueNumbers(int min = 0, int max = 1, int count = 1)
        {
            var hashSet = new HashSet<int>();

            while (hashSet.Count < count)
            {
                hashSet.Add(this.random.Next(min, max));
            }

            return hashSet;
        }

        public string GenerateRandomString(int length)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public string GenerateUniqueString(string prefix)
        {
            return string.Format("{0}_{1}", prefix, Guid.NewGuid());
        }

        public Service.Dto.Contact GenerateContact()
        {
            return new Service.Dto.Contact()
            {
                Email = string.Format("{0}@{1}.com", this.GenerateRandomString(25), this.GenerateRandomString(8)),
                Web = string.Format("http://www.{0}.com", this.GenerateRandomString(15)),
                Fax = string.Format("({0}){1}-{2}-{3}", this.random.Next(10, 99), this.random.Next(100, 999), this.random.Next(10, 99), this.random.Next(10, 99)),
                Landline = string.Format("({0}){1}-{2}-{3}", this.random.Next(10, 99), this.random.Next(100, 999), this.random.Next(10, 99), this.random.Next(10, 99)),
                Mobile = string.Format("({0}){1}-{2}-{3}", this.random.Next(10, 99), this.random.Next(100, 999), this.random.Next(100, 990), this.random.Next(1000, 9999)),
            };
        }

        public Service.Dto.Address GenerateAddress()
        {
            return new Service.Dto.Address()
            {
                Barangay = "Barangay_" + this.GenerateRandomString(10),
                City = "City_" + this.GenerateRandomString(10),
                Country = "Philippines",
                Province = "Province_" + this.GenerateRandomString(10),
                Region = "Region_" + this.GenerateRandomString(10),
                Street = "Street_" + this.GenerateRandomString(10),
                ZipCode = this.random.Next(1000, 9999).ToString()
            };
        }

        public Service.Dto.Branch GenerateBranch()
        {
            return new Service.Dto.Branch()
            {
                Address = this.GenerateAddress(),
                Description = "Description_" + Guid.NewGuid().ToString(),
                Name = "Name_" + this.GenerateRandomString(25)
            };
        }

        public Service.Dto.Person GeneratePerson()
        {
            return new Service.Dto.Person()
            {
                FirstName = "FirstName_" + this.GenerateRandomString(10),
                LastName = "LastName_" + this.GenerateRandomString(10),
                MiddleName = "MiddleNam_" + this.GenerateRandomString(10),
                BirthDate = new DateTime(this.random.Next(1930, 1995), this.random.Next(1, 12), this.random.Next(1, 28))
            };
        }

        public Service.Dto.User GenerateUser()
        {
            return new Service.Dto.User()
            {
                //Id = "Id_" + this.GenerateRandomString(25),
                Password = "Password_" + this.GenerateRandomString(15),
                Username = "Username_" + this.GenerateRandomString(15),
                Address = this.GenerateAddress(),
                BranchId = this.GenerateBranch().Id,
                Person = this.GeneratePerson(),
                Roles = Role.All
                    .Select(t => new Service.Dto.Role
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Assigned = true
                    })
                    .ToList()
            };
        }

        public Service.Dto.Customer GenerateCustomer()
        {
            return new Service.Dto.Customer()
            {
                BillingAddress = this.GenerateAddress(),
                Contact = this.GenerateContact(),
                CreditLimitAmount = 1000.00M,
                Id = this.GenerateUniqueString("Id_"),
                Name = this.GenerateUniqueString("Name_"),
                OfficeAddress = this.GenerateAddress(),
                //PricingSchemeId = 
            };
        }

        public Service.Dto.Supplier GenerateSupplier()
        {
            return new Service.Dto.Supplier()
            {
                Address = this.GenerateAddress(),
                Contact = this.GenerateContact(),
                Name = "Name_" + this.GenerateRandomString(25)
            };
        }

        public Service.Dto.Product GenerateProduct(Lookup<string> category, Lookup<Guid> supplier)
        {
            var basePrice = (decimal)random.Next(10, 100) / 100;
            return new Service.Dto.Product()
            {
                Category = category,
                Supplier = supplier,
                Description = "Description_" + this.GenerateRandomString(25),
                Discontinued = false,
                Image = "",
                Name = "Name_" + this.GenerateRandomString(10),
                Inventory = new Service.Dto.Inventory()
                {
                    BasePriceAmount = basePrice,
                    WholesalePriceAmount = basePrice + 1,
                    RetailPriceAmount = basePrice + 2,
                }
            };
        }

    }
}
