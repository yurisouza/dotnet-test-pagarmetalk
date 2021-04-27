using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PagarMeTalk.Api.Models
{
    public class PaidOrderModel
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [Required]
        public long TotalPaidInCents { get; set; }
    }
}