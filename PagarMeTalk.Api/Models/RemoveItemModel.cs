using Newtonsoft.Json;
using System;

namespace PagarMeTalk.Api.Models
{
    public class RemoveItemModel
    {
        public RemoveItemModel(Guid id, Guid orderId)
        {
            Id = id;
            OrderId = orderId;
        }

        [JsonIgnore]
        public Guid Id { get; set; }
        [JsonIgnore]
        public Guid OrderId { get; set; }
    }
}