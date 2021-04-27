using FluentAssertions;
using Moq;
using PagarMeTalk.Api.Entities;
using PagarMeTalk.Api.Entities.Enum;
using PagarMeTalk.Api.Models;
using PagarMeTalk.Api.Models.Output;
using PagarMeTalk.Api.Repositories;
using PagarMeTalk.Api.Services;
using PagarMeTalk.Api.Shared;
using PagarMeTalk.UnitTests.FakerData;
using System;
using Xunit;

namespace PagarMeTalk.UnitTests
{
    [Trait("Services", "Order")]
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _repositoryMock;
        
        private readonly IOrderService _service;

        public OrderServiceTests()
        {
            _repositoryMock = new Mock<IOrderRepository>();
            _service = new OrderService(_repositoryMock.Object);
        }

        [Fact(DisplayName = "DADO o início de um pedido QUANDO serviço for chamado ENTÃO devo ter um pedido criado.")]
        public void ShouldBeSuccessToBeginOrder()
        {
            //Arrange
            _repositoryMock.Setup(r => r.Insert(It.IsAny<Order>())).Returns(FakerOrder.GetSample());

            //Act
            var response = _service.BeginOrder();

            //Assert
            var output = GlobalMapper.Map<Result<OrderModelOutput>>(response);

            var validateTest = response.Success &&
                               output.Data.Status == EOrderStatus.Pending;

            _repositoryMock.Verify(r => r.Insert(It.IsAny<Order>()), Times.Once);

            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "DADO um Pedido pendente QUANDO fechar o pedido ENTÃO devo ter um pedido fechado.")]
        public void ShouldBeSuccessToCloseOrder()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(3);

            _repositoryMock.Setup(r => r.Get(It.Is<Guid>(id => id == order.Id))).Returns(order);
            _repositoryMock.Setup(r => r.Update(It.Is<Order>(o => o.Id == order.Id))).Returns(order);

            //Act
            var response = _service.CloseOrder(order.Id);

            //Assert
            var output = GlobalMapper.Map<Result<OrderModelOutput>>(response);

            var validateTest = response.Success &&
                               output.Data.Status == EOrderStatus.WaitingPayment;

            _repositoryMock.Verify(r => r.Get(It.Is<Guid>(id => id == order.Id)), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.Is<Order>(o => o.Id == order.Id)), Times.Once);

            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "DADO um Pedido QUANDO cancelar o pedido ENTÃO devo ter um pedido cancelado.")]
        public void ShouldBeSuccessToCancelOrder()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(3);

            _repositoryMock.Setup(r => r.Get(It.Is<Guid>(id => id == order.Id))).Returns(order);
            _repositoryMock.Setup(r => r.Update(It.Is<Order>(o => o.Id == order.Id))).Returns(order);

            //Act
            var response = _service.CancelOrder(order.Id);

            //Assert
            var output = GlobalMapper.Map<Result<OrderModelOutput>>(response);

            var validateTest = response.Success &&
                               output.Data.Status == EOrderStatus.Canceled;

            _repositoryMock.Verify(r => r.Get(It.Is<Guid>(id => id == order.Id)), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.Is<Order>(o => o.Id == order.Id)), Times.Once);

            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "DADO um Pedido aguardando pagamento QUANDO pago ENTÃO devo ter um pedido pago.")]
        public void ShouldBeSuccessToPaidOrder()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(3);
            order.Close();

            var model = new PaidOrderModel()
            {
                Id = order.Id,
                TotalPaidInCents = order.TotalInCents
            };

            _repositoryMock.Setup(r => r.Get(It.Is<Guid>(id => id == order.Id))).Returns(order);
            _repositoryMock.Setup(r => r.Update(It.Is<Order>(o => o.Id == order.Id))).Returns(order);

            //Act
            var response = _service.PaidOrder(model);

            //Assert
            var output = GlobalMapper.Map<Result<OrderModelOutput>>(response);

            var validateTest = response.Success &&
                               output.Data.Status == EOrderStatus.Paid;

            _repositoryMock.Verify(r => r.Get(It.Is<Guid>(id => id == order.Id)), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.Is<Order>(o => o.Id == order.Id)), Times.Once);

            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "DADO um Pedido aguardando pagamento QUANDO pago um valor maior ENTÃO devo ter um pedido overpaid.")]
        public void ShouldBeSuccessToOverpaidOrder()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(3);
            order.Close();

            var model = new PaidOrderModel()
            {
                Id = order.Id,
                TotalPaidInCents = order.TotalInCents + 100
            };

            _repositoryMock.Setup(r => r.Get(It.Is<Guid>(id => id == order.Id))).Returns(order);
            _repositoryMock.Setup(r => r.Update(It.Is<Order>(o => o.Id == order.Id))).Returns(order);

            //Act
            var response = _service.PaidOrder(model);

            //Assert
            var output = GlobalMapper.Map<Result<OrderModelOutput>>(response);

            var validateTest = response.Success &&
                               output.Data.Status == EOrderStatus.Overpaid;

            _repositoryMock.Verify(r => r.Get(It.Is<Guid>(id => id == order.Id)), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.Is<Order>(o => o.Id == order.Id)), Times.Once);

            validateTest.Should().BeTrue();
        }

        [Fact(DisplayName = "DADO um Pedido aguardando pagamento QUANDO pago um valor menor ENTÃO devo ter um pedido underpaid.")]
        public void ShouldBeSuccessToUnderpaidOrder()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(3);
            order.Close();

            var model = new PaidOrderModel()
            {
                Id = order.Id,
                TotalPaidInCents = order.TotalInCents - 100
            };

            _repositoryMock.Setup(r => r.Get(It.Is<Guid>(id => id == order.Id))).Returns(order);
            _repositoryMock.Setup(r => r.Update(It.Is<Order>(o => o.Id == order.Id))).Returns(order);

            //Act
            var response = _service.PaidOrder(model);

            //Assert
            var output = GlobalMapper.Map<Result<OrderModelOutput>>(response);

            var validateTest = response.Success &&
                               output.Data.Status == EOrderStatus.Underpaid;

            _repositoryMock.Verify(r => r.Get(It.Is<Guid>(id => id == order.Id)), Times.Once);
            _repositoryMock.Verify(r => r.Update(It.Is<Order>(o => o.Id == order.Id)), Times.Once);

            validateTest.Should().BeTrue();
        }
    }
}