using Microsoft.Extensions.Caching.Memory;
using PagarMeTalk.Api.Entities;
using System;
using PagarMeTalk.Api.Shared;
using Newtonsoft.Json;

namespace PagarMeTalk.Api.Repositories
{
    public interface IOrderRepository
    {
        Order Insert(Order order);
        Order Update(Order order);
        Order Get(Guid id);
        bool Exist(Guid id);
        void Remove(Guid id);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly IMemoryCache _db;

        public OrderRepository(IMemoryCache db)
        {
            _db = db;
        }

        public bool Exist(Guid id)
        {
            return _db.TryGetValue(id, out _);
        }

        public Order Get(Guid id)
        {
            return _db.Get<Order>(id)?.Clone();
        }

        public Order Insert(Order order)
        {
            return _db.Set(order.Id, order);
        }

        public void Remove(Guid id)
        {
            _db.Remove(id);
        }

        public Order Update(Order order)
        {
            return _db.Set(order.Id, order);
        }
    }
}
