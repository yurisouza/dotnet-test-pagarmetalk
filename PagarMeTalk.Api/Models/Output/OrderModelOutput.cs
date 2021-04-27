using PagarMeTalk.Api.Entities.Enum;
using System;
using System.Collections.Generic;

namespace PagarMeTalk.Api.Models.Output
{
    public class OrderModelOutput
    {
        public Guid Id { get; set; }
        public long TotalInCents { get; set; }
        public long TotalPaidInCents { get; set; }
        public EOrderStatus Status { get; set; }
        public IList<ItemModelOutput> Items { get; set; }
    }
}