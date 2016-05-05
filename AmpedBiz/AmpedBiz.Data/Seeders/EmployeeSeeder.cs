using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Core.Entities;
using NHibernate;

namespace AmpedBiz.Data.Seeders
{

    public class EmployeeSeeder //: ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public EmployeeSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool DummyData
        {
            get { return true; }
        }

        public int ExecutionOrder
        {
            get { return 27; }
        }

        public void Seed()
        {
            var data = new List<Employee>();
            /*
            for (int i = 0; i < 153; i++)
            {
                data.Add(new Employee(Guid.NewGuid())
                {
                    Address = new Address()
                    {
                        Street = $"Street {i}",
                        Barangay = $"Barangay {i}",
                        City = $"City {i}",
                        Province = $"Province {i}",
                        Region = $"Region {i}",
                        Country = $"Country {i}",
                        ZipCode = $"Zip Code {i}"
                    },
                    Branch = new Branch($"branch{i}")
                    {
                        Address = new Address()
                        {
                            Street = $"Street {i}",
                            Barangay = $"Barangay {i}",
                            City = $"City {i}",
                            Province = $"Province {i}",
                            Region = $"Region {i}",
                            Country = $"Country {i}",
                            ZipCode = $"Zip Code {i}"
                        },
                        Description = $"Description {i}",
                        Name = $"Name {i}"
                    },
                    Contact = new Contact()
                    {
                        Email = $"employee{i}@domain.com",
                        Landline = $"{i}{i}{i}-{i}{i}{i}{i}",
                        Fax = $"{i}{i}{i}-{i}{i}{i}{i}",
                        Mobile = $"{i}{i}{i}{i}-{i}{i}{i}-{i}{i}{i}{i}",
                        Web = $"employee{i}.com",
                    },
                    EmployeeType = i < 50 ? EmployeeType.Admin : i > 100 ? EmployeeType.Sales : EmployeeType.Warehouse
                    
                    
                });
            }
            /*
            /*
            for (int i = 0; i < 153; i++)
            {
                data.Add(new Employee($"customer{i}")
                {
                    Name = $"Customer {i}",
                    Tenant = new Tenant(),
                    Contact = new Contact()
                    {
                        Email = $"customer{i}@domain.com",
                        Landline = $"{i}{i}{i}-{i}{i}{i}{i}",
                        Fax = $"{i}{i}{i}-{i}{i}{i}{i}",
                        Mobile = $"{i}{i}{i}{i}-{i}{i}{i}-{i}{i}{i}{i}",
                        Web = $"customer{i}.com",
                    },
                    OfficeAddress = new Address()
                    {
                        Street = $"Street {i}",
                        Barangay = $"Barangay {i}",
                        City = $"City {i}",
                        Province = $"Province {i}",
                        Region = $"Region {i}",
                        Country = $"Country {i}",
                        ZipCode = $"Zip Code {i}"
                    },
                    BillingAddress = new Address()
                    {
                        Street = $"Street {i}",
                        Barangay = $"Barangay {i}",
                        City = $"City {i}",
                        Province = $"Province {i}",
                        Region = $"Region {i}",
                        Country = $"Country {i}",
                        ZipCode = $"Zip Code {i}"
                    }
                });
            }

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                //session.SetBatchSize(100);

                var entity = session.Query<Customer>().ToList();
                if (entity.Count == 0)
                {
                    foreach (var item in data)
                    {
                        session.Save(item);
                    }
                }

                transaction.Commit();
            }*/
        }
    }
}
