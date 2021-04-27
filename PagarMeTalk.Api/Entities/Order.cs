using FluentValidation;
using PagarMeTalk.Api.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PagarMeTalk.Api.Entities
{
    public class Order : Entity<Order>
    {
        public Guid Id { get; private set; }
        public long TotalInCents { get; private set; }
        public long TotalPaidInCents { get; private set; }
        public EOrderStatus Status { get; private set; }
        public List<Item> Items { get; private set; }

        public Order()
        {
            Id = Guid.NewGuid();
            Status = EOrderStatus.Pending;
            Items = new List<Item>();
        }

        public override bool IsValid()
        {
            ValidateTotalInCents();
            ValidateTotalPaidInCents();

            AddErrors(Validate(this));
            return ValidationResult.IsValid;
        }

        public void AddItem(Item item)
        {
            if (IsPending() == false)
            {
                AddError("Status", "Apenas um pedido aberto pode ser alterado");
                return;
            }

            if (item.IsValid())
            {
                TotalInCents += item.PriceInCents;
                Items.Add(item);
            }
            else
            {
                AddErrors(item.ValidationResult);
            }
        }

        public void RemoveItem(Guid id)
        {
            if (IsPending() == false)
            {
                AddError("Status", "Apenas um pedido aberto pode ser alterado");
                return;
            }

            var item = Items.FirstOrDefault(i => i.Id == id);

            if (item == null)
            {
                AddError("Id", "Item não encontrado");
                return;
            }

            if (Items.Remove(item))
                TotalInCents -= item.PriceInCents;
        }

        public void Paid(long amountInCents)
        {
            if (Status == EOrderStatus.Canceled)
            {
                AddError("Status", "Um pedido cancelado não pode ser pago");
                return;
            }

            if (Status != EOrderStatus.WaitingPayment)
            {
                AddError("Status", "Um pedido só pode ser pago se ele estiver fechado");
                return;
            }

            if (amountInCents > TotalInCents)
                Status = EOrderStatus.Overpaid;
            else if (amountInCents < TotalInCents)
                Status = EOrderStatus.Underpaid;
            else
                Status = EOrderStatus.Paid;

            TotalPaidInCents = amountInCents;
        }

        public void Cancel()
        {
            Status = EOrderStatus.Canceled;
        }

        public void Close()
        {
            if (IsPending() == false)
            {
                AddError("Status", "Apenas um pedido aberto pode ser fechado");
                return;
            }

            if (Items.Any() == false)
            {
                AddError("Items", "Um pedido só pode ser fechado com pelo menos 1 item");
                return;
            }

            Status = EOrderStatus.WaitingPayment;
        }

        private void ValidateTotalInCents()
        {
            RuleFor(p => p.TotalInCents)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O total do pedido deve ser maior que zero");
        }

        private void ValidateTotalPaidInCents()
        {
            RuleFor(p => p.TotalPaidInCents)
                .GreaterThanOrEqualTo(0)
                .When(p => p.Status == EOrderStatus.Paid || p.Status == EOrderStatus.Overpaid || p.Status == EOrderStatus.Underpaid);
        }

        private bool IsClosed() => Status == EOrderStatus.WaitingPayment;

        private bool IsPending() => Status == EOrderStatus.Pending;

        public Order Clone()
        {
            return new Order()
            {
                Id = this.Id,
                Status = this.Status,
                TotalInCents = this.TotalInCents,
                TotalPaidInCents = this.TotalPaidInCents,
                Items = this.Items
            };
        }
    }
}