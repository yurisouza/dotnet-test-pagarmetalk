using FluentAssertions;
using PagarMeTalk.Api.Entities;
using PagarMeTalk.UnitTests.FakerData;
using Xunit;

namespace PagarMeTalk.UnitTests
{
    [Trait("Entities", "Item")]
    public class ItemTests
    {
        [Fact(DisplayName = "DADO um Item v�lido QUANDO for validado ENT�O deve ser v�lido.")]
        public void ShouldBeAValidItem()
        {
            //Arrange
            var item = FakerItem.GetSample();

            //Act
            var itemTest = item.IsValid();

            //Assert
            itemTest.Should().BeTrue();
        }

        [Fact(DisplayName = "DADO um Item sem nome QUANDO for validado ENT�O deve ser inv�lido.")]
        public void ShouldBeAInvalidItemByEmptyName()
        {
            //Arrange
            var item = FakerItem.GetInvalidSampleByEmptyName();

            //Act
            var itemTest = item.IsValid();

            //Assert
            itemTest.Should().BeFalse();
        }

        [Fact(DisplayName = "DADO um Item sem pre�o QUANDO for validado ENT�O deve ser inv�lido.")]
        public void ShouldBeAInvalidItemByZeroedPrice()
        {
            //Arrange
            var item = FakerItem.GetInvalidSampleByZeroedPrice();

            //Act
            var itemTest = item.IsValid();

            //Assert
            itemTest.Should().BeFalse();
        }

        [Theory(DisplayName = "DADO um Item inv�lido QUANDO for validado ENT�O deve ser inv�lido.")]
        [MemberData(nameof(FakerItem.GetSamplesWithInvalidInputs), MemberType = typeof(FakerItem))]
        public void ShouldBeAInvalidItem(Item item)
        {
            //Act
            var itemTest = item.IsValid();

            //Assert
            itemTest.Should().BeFalse();
        }

        [Theory(DisplayName = "DADO um Item inv�lido QUANDO for validado ENT�O deve ser inv�lido forma 2.")]
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
