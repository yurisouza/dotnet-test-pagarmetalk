using FluentAssertions;
using PagarMeTalk.Api.Entities;
using PagarMeTalk.UnitTests.FakerData;
using Xunit;

namespace PagarMeTalk.UnitTests
{
    [Trait("Entities", "Item")]
    public class ItemTests
    {
        [Fact(DisplayName = "DADO um Item válido QUANDO for validado ENTÃO deve ser válido.")]
        public void ShouldBeAValidItem()
        {
            //Arrange
            var item = FakerItem.GetSample();

            //Act
            var itemTest = item.IsValid();

            //Assert
            itemTest.Should().BeTrue();
        }

        [Fact(DisplayName = "DADO um Item sem nome QUANDO for validado ENTÃO deve ser inválido.")]
        public void ShouldBeAInvalidItemByEmptyName()
        {
            //Arrange
            var item = FakerItem.GetInvalidSampleByEmptyName();

            //Act
            var itemTest = item.IsValid();

            //Assert
            itemTest.Should().BeFalse();
        }

        [Fact(DisplayName = "DADO um Item sem preço QUANDO for validado ENTÃO deve ser inválido.")]
        public void ShouldBeAInvalidItemByZeroedPrice()
        {
            //Arrange
            var item = FakerItem.GetInvalidSampleByZeroedPrice();

            //Act
            var itemTest = item.IsValid();

            //Assert
            itemTest.Should().BeFalse();
        }

        [Theory(DisplayName = "DADO um Item inválido QUANDO for validado ENTÃO deve ser inválido.")]
        [MemberData(nameof(FakerItem.GetSamplesWithInvalidInputs), MemberType = typeof(FakerItem))]
        public void ShouldBeAInvalidItem(Item item)
        {
            //Act
            var itemTest = item.IsValid();

            //Assert
            itemTest.Should().BeFalse();
        }

        [Theory(DisplayName = "DADO um Item inválido QUANDO for validado ENTÃO deve ser inválido forma 2.")]
        [InlineData("Item 1", 0)]
        [InlineData("", 990)]
        public void ShouldBeAInvalidItem2(string name, long priceInCents)
        {
            //Arrange
            var item = new Item(name, priceInCents);

            //Act
            var itemTest = item.IsValid();

            //Assert
            itemTest.Should().BeFalse();
        }
    }
}
