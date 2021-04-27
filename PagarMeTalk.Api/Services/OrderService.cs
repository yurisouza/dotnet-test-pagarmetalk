using AutoMapper;
using PagarMeTalk.Api.Entities;
using PagarMeTalk.Api.Models;
using PagarMeTalk.Api.Models.Output;
using PagarMeTalk.Api.Repositories;
using PagarMeTalk.Api.Shared;
using System;

namespace PagarMeTalk.Api.Services
{
    public interface IOrderService
    {
        IResult GetOrder(Guid id);
        IResult BeginOrder();
        IResult CloseOrder(Guid id);
        IResult AddItem(AddItemModel model);
        IResult RemoveItem(RemoveItemModel model);
        IResult PaidOrder(PaidOrderModel model);
        IResult CancelOrder(Guid id);
    }

    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _mapper = GlobalMapper.Mapper;
            _repository = repository;
        }

        public IResult AddItem(AddItemModel model)
        {
            var order = _repository.Get(model.OrderId);

            if (order == null)
                return ErrorExtensions.CreateNotification("id", "Nenhum pedido encontrado");

            var item = _mapper.Map<Item>(model);
            order.AddItem(item);

            if (order.IsValid())
            {
                _repository.Update(order);
                var output = _mapper.Map<ItemModelOutput>(item);
                return new Result<ItemModelOutput>(output, "Item adicionado com sucesso!", true);
            }

            return order.GetNotifications();
        }

        public IResult CancelOrder(Guid id)
        {
            var order = _repository.Get(id);

            if (order == null)
                return ErrorExtensions.CreateNotification("id", "Nenhum pedido encontrado");

            order.Cancel();

            if (order.IsValid())
            {
                _repository.Update(order);
                var output = _mapper.Map<OrderModelOutput>(order);
                return new Result<OrderModelOutput>(output, "Pedido cancelado com sucesso!", true);
            }

            return order.GetNotifications();
        }

        public IResult CloseOrder(Guid id)
        {
            var order = _repository.Get(id);

            if (order == null)
                return ErrorExtensions.CreateNotification("id", "Nenhum pedido encontrado");

            order.Close();

            if (order.IsValid())
            {
                _repository.Update(order);
                var output = _mapper.Map<OrderModelOutput>(order);
                return new Result<OrderModelOutput>(output, "Pedido fechado com sucesso!", true);
            }

            return order.GetNotifications();
        }

        public IResult GetOrder(Guid id)
        {
            var order = _repository.Get(id);

            if (order == null)
                return ErrorExtensions.CreateNotification("id", "Nenhum pedido encontrado");

            var output = _mapper.Map<OrderModelOutput>(order);

            return new Result<OrderModelOutput>(output, "Pedido obtido com sucesso!", true);
        }

        public IResult BeginOrder()
        {
            var order = _repository.Insert(new Order());

            var output = _mapper.Map<OrderModelOutput>(order);

            return new Result<OrderModelOutput>(output, "Pedido iniciado com sucesso!", true);
        }

        public IResult PaidOrder(PaidOrderModel model)
        {
            var order = _repository.Get(model.Id);

            if (order == null)
                return ErrorExtensions.CreateNotification("id", "Nenhum pedido encontrado");

            order.Paid(model.TotalPaidInCents);

            if (order.IsValid())
            {
                _repository.Update(order);
                var output = _mapper.Map<OrderModelOutput>(order);
                return new Result<OrderModelOutput>(output, "Pedido pago com sucesso!", true);
            }

            return order.GetNotifications();
        }

        public IResult RemoveItem(RemoveItemModel model)
        {
            var order = _repository.Get(model.OrderId);

            if (order == null)
                return ErrorExtensions.CreateNotification("id", "Nenhum pedido encontrado");

            order.RemoveItem(model.Id);

            if (order.IsValid())
            {
                _repository.Update(order);
                var output = _mapper.Map<OrderModelOutput>(order);
                return new Result<OrderModelOutput>(output, "Item removido com sucesso!", true);
            }

            return order.GetNotifications();
        }
    }
}
