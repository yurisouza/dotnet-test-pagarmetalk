using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PagarMeTalk.Api.Models
{
    public class AddItemModel
    {
        [JsonIgnore]
        public Guid OrderId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public long PriceInCents { get; set; }
    }
}