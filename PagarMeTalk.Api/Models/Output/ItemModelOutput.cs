using System;

namespace PagarMeTalk.Api.Models.Output
{
    public class ItemModelOutput
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long PriceInCents { get; set; }
    }
}