using AmpedBiz.Pos.Common.Models;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmpedBiz.Pos.Common.Services
{
    public interface IApiService
    {
        Task<IReadOnlyList<User>> Users();
        Task<IReadOnlyList<Branch>> Branches();
        Task<IReadOnlyList<Product>> Products();
        Task<IReadOnlyList<Customer>> Customers();
        Task<IReadOnlyList<UnitOfMeasure>> UnitOfMeasures();
    }

    public class FakeApiService : IApiService
    {
        private readonly IReadOnlyList<User> _users;
        private readonly IReadOnlyList<Branch> _branches;
        private readonly IReadOnlyList<Product> _products;
        private readonly IReadOnlyList<Customer> _customers;
        private readonly IReadOnlyList<UnitOfMeasure> _unitOfMeasures;

        public FakeApiService()
        {
            var faker = new Faker();

            this._users = Enumerable.Range(0, faker.Random.Int(2, 10))
                .Select(x => new User(Guid.NewGuid(), faker.Name.FullName()))
                .ToList().AsReadOnly();

            this._branches = Enumerable.Range(0, faker.Random.Int(1, 5))
                .Select(x => new Branch(Guid.NewGuid(), faker.Address.City()))
                .ToList().AsReadOnly();

            this._products = Enumerable.Range(0, faker.Random.Int(200, 250))
                .Select(x => new Product(Guid.NewGuid(), faker.Commerce.Product()))
                .ToList().AsReadOnly();

            this._customers = Enumerable.Range(0, faker.Random.Int(100, 150))
                .Select(x => new Customer(Guid.NewGuid(), faker.Company.CompanyName()))
                .ToList().AsReadOnly();

            var units = new[] { "Piece", "Box", "Sack", "Pack" };

            this._unitOfMeasures = Enumerable.Range(0, units.Count() - 1)
                .Select(x => new UnitOfMeasure(units[x].ToLower(), units[x]))
                .ToList().AsReadOnly();
        }

        public Task<IReadOnlyList<Branch>> Branches() => Task.FromResult(this._branches);

        public Task<IReadOnlyList<Customer>> Customers() => Task.FromResult(this._customers);

        public Task<IReadOnlyList<Product>> Products() => Task.FromResult(this._products);

        public Task<IReadOnlyList<UnitOfMeasure>> UnitOfMeasures() => Task.FromResult(this._unitOfMeasures);

        public Task<IReadOnlyList<User>> Users() => Task.FromResult(this._users);
    }
}
