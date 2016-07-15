﻿using System;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Orders
{
    public class CompleteOrder
    {
        public class Request : Dto.Order, IRequest<Response>
        {
            public virtual Guid UserId { get; set; }
        }

        public class Response : Dto.Order { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory)
            {
            }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<Order>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Order with id {message.Id} does not exists.");

                    var user = session.Load<User>(message.UserId);

                    entity.State.Complete(user);

                    session.Save(entity);
                    transaction.Commit();

                    //todo: not working when mapped to invoice
                    //entity.MapTo(response);
                    response.Id = entity.Id;
                    response.Status = entity.Status == OrderStatus.Completed ? Dto.OrderStatus.Completed : Dto.OrderStatus.New;
                }

                return response;
            }
        }
    }
}