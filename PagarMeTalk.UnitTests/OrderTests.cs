using FluentAssertions;
using PagarMeTalk.Api.Entities;
using PagarMeTalk.Api.Entities.Enum;
using PagarMeTalk.UnitTests.FakerData;
using System;
using System.Linq;
using Xunit;

namespace PagarMeTalk.UnitTests
{
    [Trait("Entities", "Order")]
    public class OrderTests
    {
        [Fact(DisplayName = "DADO um Pedido válido QUANDO for validado ENTÃO deve ser válido.")]
        public void ShouldBeAValidOrder()
        {
            //Arrange
            var order = FakerOrder.GetSample();

            //Act
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeTrue();
            order.Status.Should().Be(EOrderStatus.Pending);
            order.Items.Should().BeEmpty();
        }

        [Theory(DisplayName = "DADO um Pedido válido e com items QUANDO for validado ENTÃO deve ser válido.")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void ShouldBeAValidOrderWithItems(int quantityOfItems)
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(quantityOfItems);

            //Act
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeTrue();
            order.Status.Should().Be(EOrderStatus.Pending);
            order.Items.Should().HaveCount(quantityOfItems);
        }

        [Fact(DisplayName = "DADO um Pedido sem items QUANDO o pedido for fechado ENTÃO deve retornar um erro.")]
        public void ShouldBeErrorToCloseOrderWithoutItems()
        {
            //Arrange
            var order = FakerOrder.GetSample();

            //Act
            order.Close();

            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeFalse();
            order.Status.Should().Be(EOrderStatus.Pending);
            order.Items.Should().BeEmpty();
        }

        [Fact(DisplayName = "DADO um Pedido com items QUANDO o pedido for fechado ENTÃO deve ter sucesso.")]
        public void ShouldBeSuccessToCloseOrderWithItems()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(1);

            //Act
            order.Close();

            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeTrue();
            order.Status.Should().Be(EOrderStatus.WaitingPayment);
            order.Items.Should().HaveCount(1);
        }

        [Theory(DisplayName = "DADO um Pedido com items e com o status diferente de pendente QUANDO o pedido for fechado ENTÃO deve retornar um erro.")]
        [InlineData(EOrderStatus.Canceled)]
        [InlineData(EOrderStatus.Overpaid)]
        [InlineData(EOrderStatus.Paid)]
        [InlineData(EOrderStatus.Underpaid)]
        [InlineData(EOrderStatus.WaitingPayment)]
        public void ShouldBeInvalidToCloseOrderNoPendingWithItems(EOrderStatus status)
        {
            //Arrange
            var order = FakerOrder.GetSampleWithStatusEqualsTo(status);

            //Act
            order.Close();

            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeFalse();
            order.Status.Should().Be(status);
            order.Items.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "DADO um Pedido pendente QUANDO for adicionado um item válido ENTÃO deve ter sucesso.")]
        public void ShouldBeSuccessToAddItem()
        {
            //Arrange
            var order = FakerOrder.GetSample();
            var item = FakerItem.GetSample();

            //Act
            order.AddItem(item);
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeTrue();
            order.Items.Should().HaveCount(1);
        }

        [Theory(DisplayName = "DADO um Pedido não pendente QUANDO for adicionado um item válido ENTÃO deve retornar um erro.")]
        [InlineData(EOrderStatus.Canceled)]
        [InlineData(EOrderStatus.Overpaid)]
        [InlineData(EOrderStatus.Paid)]
        [InlineData(EOrderStatus.Underpaid)]
        [InlineData(EOrderStatus.WaitingPayment)]
        public void ShouldInvalidToAddItemWithOrderNoPending(EOrderStatus status)
        {
            //Arrange
            var order = FakerOrder.GetSampleWithStatusEqualsTo(status);
            var countItems = order.Items.Count();
            var item = FakerItem.GetSample();

            //Act
            order.AddItem(item);
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeFalse();
            order.Items.Should().HaveCount(countItems);
        }

        [Theory(DisplayName = "DADO um Pedido pendente QUANDO for adicionado um item inválido ENTÃO deve retornar um erro.")]
        [MemberData(nameof(FakerItem.GetSamplesWithInvalidInputs), MemberType = typeof(FakerItem))]
        public void ShouldBeInvalidToAddItem(Item item)
        {
            //Arrange
            var order = FakerOrder.GetSample();

            //Act
            order.AddItem(item);
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeFalse();
            order.Items.Should().BeEmpty();
        }

        [Fact(DisplayName = "DADO um Pedido pendente QUANDO for removido um item ENTÃO deve ter sucesso.")]
        public void ShouldBeSuccessToRemoveItem()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(3);
            var item = FakerItem.GetSample();

            //Act
            order.AddItem(item);
            order.RemoveItem(item.Id);
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeTrue();
            order.Items.Should().HaveCount(3);
        }

        [Theory(DisplayName = "DADO um Pedido não pendente QUANDO for removido um item ENTÃO deve retornar um erro.")]
        [InlineData(EOrderStatus.Canceled)]
        [InlineData(EOrderStatus.Overpaid)]
        [InlineData(EOrderStatus.Paid)]
        [InlineData(EOrderStatus.Underpaid)]
        [InlineData(EOrderStatus.WaitingPayment)]
        public void ShouldBeInvalidToRemoveItemWithOrderNoPending(EOrderStatus status)
        {
            //Arrange
            var order = FakerOrder.GetSampleWithStatusEqualsTo(status);
            var item = order.Items.FirstOrDefault();

            //Act
            order.RemoveItem(item.Id);
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeFalse();
            order.Items.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "DADO um Pedido pendente QUANDO for removido um item inexistente ENTÃO deve retornar um erro.")]
        public void ShouldBeInvalidToRemoveItem()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(2);

            //Act
            order.RemoveItem(Guid.NewGuid());
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeFalse();
            order.Items.Should().HaveCount(2);
        }

        [Fact(DisplayName = "DADO um Pedido fechado QUANDO for pago ENTÃO deve ter sucesso.")]
        public void ShouldBeSuccessToPaidOrder()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(3);
            order.Close();

            //Act
            var totalPaid = order.TotalInCents;
            order.Paid(totalPaid);
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeTrue();
            order.TotalPaidInCents.Should().Be(totalPaid);
            order.Status.Should().Be(EOrderStatus.Paid);
        }

        [Fact(DisplayName = "DADO um Pedido fechado QUANDO for pago a maior ENTÃO deve ter sucesso.")]
        public void ShouldBeSuccessToOverpaidOrder()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(3);
            order.Close();

            //Act
            var totalPaid = order.TotalInCents + 100;
            order.Paid(totalPaid);
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeTrue();
            order.TotalPaidInCents.Should().Be(totalPaid);
            order.Status.Should().Be(EOrderStatus.Overpaid);
        }

        [Fact(DisplayName = "DADO um Pedido fechado QUANDO for pago a menor ENTÃO deve ter sucesso.")]
        public void ShouldBeSuccessToUnderpaidOrder()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(3);
            order.Close();

            //Act
            var totalPaid = order.TotalInCents - 100;
            order.Paid(totalPaid);
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeTrue();
            order.TotalPaidInCents.Should().Be(totalPaid);
            order.Status.Should().Be(EOrderStatus.Underpaid);
        }

        [Fact(DisplayName = "DADO um Pedido cancelado QUANDO for pago ENTÃO deve retornar um erro.")]
        public void ShouldBeInvalidToOrderCanceled()
        {
            //Arrange
            var order = FakerOrder.GetSampleWithItems(3);
            order.Cancel();

            //Act
            var totalPaid = order.TotalInCents;
            order.Paid(totalPaid);
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeFalse();
            order.TotalPaidInCents.Should().Be(0);
            order.Status.Should().Be(EOrderStatus.Canceled);
        }

        [Theory(DisplayName = "DADO um Pedido com status diferente de aguardando pagamento QUANDO for pago ENTÃO deve retornar um erro.")]
        [InlineData(EOrderStatus.Canceled)]
        [InlineData(EOrderStatus.Pending)]
        public void ShouldBeInvalidToOrderWithStatusDifferentOfWaitingPayment(EOrderStatus status)
        {
            //Arrange
            var order = FakerOrder.GetSampleWithStatusEqualsTo(status);

            //Act
            var totalPaid = order.TotalInCents;
            order.Paid(totalPaid);
            var orderTest = order.IsValid();

            //Assert
            orderTest.Should().BeFalse();
            order.TotalPaidInCents.Should().Be(0);
            order.Status.Should().Be(status);
        }
    }
}