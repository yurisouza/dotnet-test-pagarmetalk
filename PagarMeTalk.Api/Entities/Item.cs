using FluentValidation;
using System;

namespace PagarMeTalk.Api.Entities
{
    public class Item : Entity<Item>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public long PriceInCents { get; private set; }

        public Item(string name, long priceInCents)
        {
            Id = Guid.NewGuid();
            Name = name;
            PriceInCents = priceInCents;
        }

        protected Item()
        {
        }

        public override bool IsValid()
        {
            ValidateName();
            ValidatePriceInCents();

            AddErrors(Validate(this));
            return ValidationResult.IsValid;
        }

        private void ValidateName()
        {
            RuleFor(p => p.Name)
                .Length(3, 1024)
                .WithMessage("O nome do item deve ter entre 3 e 1024 caracteres.");
        }

        private void ValidatePriceInCents()
        {
            RuleFor(p => p.PriceInCents)
                .GreaterThan(0)
                .WithMessage("O preço do item deve ser maior que zero");
        }
    }
}