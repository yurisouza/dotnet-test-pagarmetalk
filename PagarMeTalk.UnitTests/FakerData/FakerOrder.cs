using Bogus;
using PagarMeTalk.Api.Entities;
using PagarMeTalk.Api.Entities.Enum;
using System;

namespace PagarMeTalk.UnitTests.FakerData
{
    public static class FakerOrder
    {
        public static Order GetSample()
        {
            return new Faker<Order>()
                    .CustomInstantiator(i => new Order());
        }

        public static Order GetSampleWithItems(int countItems)
        {
            return new Faker<Order>()
                    .CustomInstantiator(i => new Order())
                    .FinishWith((faker, order) =>
                    {
                        var items = FakerItem.GetSamples(countItems);

                        foreach (var item in items)
                            order.AddItem(item);
                    });
        }

        public static Order GetSampleWithStatusEqualsTo(EOrderStatus status)
        {
            var order = GetSampleWithItems(3);

            switch (status)
            {
                case EOrderStatus.Canceled:
                    order.Cancel();
                    break;

                case EOrderStatus.Overpaid:
                    order.Close();
                    order.Paid(order.TotalInCents + 100);
                    break;

                case EOrderStatus.Underpaid:
                    order.Close();
                    order.Paid(order.TotalInCents - 100);
                    break;

                case EOrderStatus.Paid:
                    order.Close();
                    order.Paid(order.TotalInCents);
                    break;

                case EOrderStatus.WaitingPayment:
                    order.Close();
                    break;

                case EOrderStatus.Pending:
                    break;

                default:
                    throw new Exception("Status don't exists");
            }

            return order;
        }

        public static Order GetSampleWithStatusDifferentOf(EOrderStatus status)
        {
            var order = GetSampleWithItems(3);
            var newStatus = FakerHelper.RandomEnumValue(status);

            switch (newStatus)
            {
                case EOrderStatus.Canceled:
                    order.Cancel();
                    break;

                case EOrderStatus.Overpaid:
                    order.Close();
                    order.Paid(order.TotalInCents + 100);
                    break;

                case EOrderStatus.Underpaid:
                    order.Close();
                    order.Paid(order.TotalInCents - 100);
                    break;

                case EOrderStatus.Paid:
                    order.Close();
                    order.Paid(order.TotalInCents);
                    break;

                case EOrderStatus.WaitingPayment:
                    order.Close();
                    break;

                case EOrderStatus.Pending:
                    break;

                default:
                    throw new Exception("Status don't exists");
            }

            return order;
        }
    }
}