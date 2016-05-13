using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Core.Entities;

namespace AmpedBiz.Tests.IntegrationTests
{
    internal class DummyData
    {
        private Random rnd = new Random();
        public string GenerateRandomString(int length)
        {
            const string allowedChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rnd.Next(0, allowedChars.Length)];
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
                Fax = string.Format("({0}){1}-{2}-{3}", this.rnd.Next(10, 99), this.rnd.Next(100, 999), this.rnd.Next(10, 99), this.rnd.Next(10, 99)),
                Landline = string.Format("({0}){1}-{2}-{3}", this.rnd.Next(10, 99), this.rnd.Next(100, 999), this.rnd.Next(10, 99), this.rnd.Next(10, 99)),
                Mobile = string.Format("({0}){1}-{2}-{3}", this.rnd.Next(10, 99), this.rnd.Next(100, 999), this.rnd.Next(100, 990), this.rnd.Next(1000, 9999)),
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
                ZipCode = this.rnd.Next(1000, 9999).ToString()
            };
        }

        public Service.Dto.Branch GenerateBranch()
        {
            return new Service.Dto.Branch()
            {
                Address = this.GenerateAddress(),
                Description = "Description_" + Guid.NewGuid().ToString(),
                Id = "Id_" + this.GenerateRandomString(10),
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
                BirthDate = new DateTime(this.rnd.Next(1930, 1995), this.rnd.Next(1, 12), this.rnd.Next(1, 31))
            };
        }

        public Service.Dto.User GenerateUser()
        {
            return new Service.Dto.User()
            {
                Id = "Id_" + this.GenerateRandomString(25),
                Password = "Password_" + this.GenerateRandomString(15),
                Username = "Username_" + this.GenerateRandomString(10),
                Address = this.GenerateAddress(),
                BranchId = this.GenerateBranch().Id,
                Person = this.GeneratePerson(),
                Roles = EmployeeType.All.Select(t => new Service.Dto.Role
                {
                    Id = t.Id,
                    Name = t.Name,
                    Assigned = true
                }).ToList()
            };
        }
    }
}
