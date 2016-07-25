﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Core.Entities;
using AmpedBiz.Common.Extentions;
using NHibernate;
using NHibernate.Linq;
using AmpedBiz.Core.Events.Orders;

namespace AmpedBiz.Data.Seeders
{
    public class OrderSeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public OrderSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool IsDummyData
        {
            get { return true; }
        }

        public int ExecutionOrder
        {
            get { return 25; }
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                //session.SetBatchSize(100);
                var productIndex = 0;
                var products = session.Query<Product>().ToList();

                Func<Product> RotateProduct = () =>
                {
                    var result = products[productIndex];

                    if (productIndex < products.Count - 1)
                        productIndex++;
                    else
                        productIndex = 0;

                    return result;
                };

                var paymentTypeIndex = 0;
                var paymentTypes = session.Query<PaymentType>().ToList();

                Func<PaymentType> RotatePaymentType = () =>
                {
                    var result = paymentTypes[paymentTypeIndex];

                    if (paymentTypeIndex < paymentTypes.Count - 1)
                        paymentTypeIndex++;
                    else
                        paymentTypeIndex = 0;

                    return result;
                };

                var shipperIndex = 0;
                var shippers = session.Query<Shipper>().ToList();

                Func<Shipper> RotateShipper = () =>
                {
                    var result = shippers[shipperIndex];

                    if (shipperIndex < shippers.Count - 1)
                        shipperIndex++;
                    else
                        shipperIndex = 0;

                    return result;
                };

                var customerIndex = 0;
                var customers = session.Query<Customer>().ToList();

                Func<Customer> RotateCustomer = () =>
                {
                    var result = customers[customerIndex];

                    if (customerIndex < customers.Count - 1)
                        customerIndex++;
                    else
                        customerIndex = 0;

                    return result;
                };

                var branchIndex = 0;
                var branches = session.Query<Branch>().ToList();

                Func<Branch> RotateBranch = () =>
                {
                    var result = branches[branchIndex];

                    if (branchIndex < branches.Count - 1)
                        branchIndex++;
                    else
                        branchIndex = 0;

                    return result;
                };

                var userIndex = 0;
                var users = session.Query<User>().ToList();

                Func<User> RotateUser = () =>
                {
                    var result = users[userIndex];

                    if (userIndex < users.Count - 1)
                        userIndex++;
                    else
                        userIndex = 0;

                    return result;
                };

                var random = new Random();

                var entity = session.Query<Order>().ToList();
                if (entity.Count == 0)
                {
                    for (int i = 0; i < 153; i++)
                    {
                        var order = new Order(Guid.NewGuid());
                        var newlyCreatedEvent = new OrderNewlyCreatedEvent(
                            createdBy: RotateUser(),
                            createdOn: DateTime.Now,
                            branch: RotateBranch(),
                            customer: RotateCustomer(),
                            shipper: RotateShipper(),
                            paymentType: RotatePaymentType(),
                            taxRate: random.NextDecimal(0.01M, 0.30M),
                            tax: null, // compute this
                            shippingFee: new Money(random.NextDecimal(10M, 10000M)),
                            items: Enumerable.Range(0, (int)random.NextDecimal(1M, 25M))
                                .Select(x => RotateProduct())
                                .Select(x => new OrderItem(
                                    product: x,
                                    quantity: new Measure(random.NextDecimal(1M, 100M), x.Inventory.UnitOfMeasure),
                                    discount: new Money(random.NextDecimal(100M, 500M)),
                                    unitPrice: new Money(random.NextDecimal(1000M, 100000M))
                                ))
                        );

                        order.State.Process(newlyCreatedEvent);

                        session.Save(order, order.Id);
                    }
                }

                transaction.Commit();
            }
        }
    }
}
