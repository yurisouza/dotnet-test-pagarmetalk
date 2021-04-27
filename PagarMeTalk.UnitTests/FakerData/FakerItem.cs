using Bogus;
using PagarMeTalk.Api.Entities;
using System.Collections.Generic;

namespace PagarMeTalk.UnitTests.FakerData
{
    public static class FakerItem
    {
        public static Item GetSample()
        {
            return new Faker<Item>()
                    .CustomInstantiator(i => new Item(i.Commerce.Product(), i.Random.Long(100, 100000)));
        }

        public static List<Item> GetSamples(int count)
        {
            return new Faker<Item>()
                    .CustomInstantiator(i => new Item(i.Commerce.Product(), i.Random.Long(100, 100000)))
                    .Generate(count);
        }

        public static Item GetInvalidSampleByEmptyName()
        {
            return new Faker<Item>()
                    .CustomInstantiator(i => new Item(string.Empty, i.Random.Long(100, 100000)));
        }

        public static Item GetInvalidSampleByZeroedPrice()
        {
            return new Faker<Item>()
                    .CustomInstantiator(i => new Item(i.Commerce.Product(), 0));
        }

        public static IEnumerable<object[]> GetSamplesWithInvalidInputs()
        {
            yield return new Faker<object[]>()
                .CustomInstantiator(lm => new object[] { GetInvalidSampleByEmptyName() });

            yield return new Faker<object[]>()
                .CustomInstantiator(lm => new object[] { GetInvalidSampleByZeroedPrice() });
        }
    }
}