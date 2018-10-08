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
        Task<IReadOnlyList<UserModel>> Users();
        Task<IReadOnlyList<BranchModel>> Branches();
        Task<IReadOnlyList<ProductModel>> Products();
        Task<IReadOnlyList<CustomerModel>> Customers();
        Task<IReadOnlyList<UnitOfMeasureModel>> UnitOfMeasures();
    }

    public class FakeApiService : IApiService
    {
        private readonly IReadOnlyList<UserModel> _users;
        private readonly IReadOnlyList<BranchModel> _branches;
        private readonly IReadOnlyList<ProductModel> _products;
        private readonly IReadOnlyList<CustomerModel> _customers;
        private readonly IReadOnlyList<UnitOfMeasureModel> _unitOfMeasures;

        public FakeApiService()
        {
            var faker = new Faker();

            this._users = Enumerable.Range(0, faker.Random.Int(2, 10))
                .Select(x => new UserModel(Guid.NewGuid(), faker.Name.FullName()))
                .ToList().AsReadOnly();

            this._branches = Enumerable.Range(0, faker.Random.Int(1, 5))
                .Select(x => new BranchModel(Guid.NewGuid(), faker.Address.City()))
                .ToList().AsReadOnly();

            this._products = Enumerable.Range(0, faker.Random.Int(200, 250))
                .Select(x => new ProductModel(Guid.NewGuid(), faker.Commerce.Product()))
                .ToList().AsReadOnly();

            this._customers = Enumerable.Range(0, faker.Random.Int(100, 150))
                .Select(x => new CustomerModel(Guid.NewGuid(), faker.Company.CompanyName()))
                .ToList().AsReadOnly();

            var units = new[] { "Piece", "Box", "Sack", "Pack" };

            this._unitOfMeasures = Enumerable.Range(0, units.Count() - 1)
                .Select(x => new UnitOfMeasureModel(units[x].ToLower(), units[x]))
                .ToList().AsReadOnly();
        }

        public Task<IReadOnlyList<BranchModel>> Branches() => Task.FromResult(this._branches);

        public Task<IReadOnlyList<CustomerModel>> Customers() => Task.FromResult(this._customers);

        public Task<IReadOnlyList<ProductModel>> Products() => Task.FromResult(this._products);

        public Task<IReadOnlyList<UnitOfMeasureModel>> UnitOfMeasures() => Task.FromResult(this._unitOfMeasures);

        public Task<IReadOnlyList<UserModel>> Users() => Task.FromResult(this._users);
    }
}
