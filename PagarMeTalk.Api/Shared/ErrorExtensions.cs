using Microsoft.AspNetCore.Mvc.ModelBinding;
using PagarMeTalk.Api.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PagarMeTalk.Api.Shared
{
    public static class ErrorExtensions
    {
        public static Result<ICollection<DomainNotification>> GetNotifications<T>(this T entity) where T : Entity<T>
        {
            var errors = entity.ValidationResult.Errors.Select(e => new DomainNotification(e.PropertyName, e.ErrorMessage));
            var notifications = new List<DomainNotification>(errors);

            return new Result<ICollection<DomainNotification>>(notifications, "Ocorreu um erro", false);
        }

        public static Result<ICollection<DomainNotification>> GetNotifications(this ModelStateDictionary modelState)
        {
            var errors = modelState.Select(m => new DomainNotification(m.Key, m.Value.Errors.FirstOrDefault()?.ErrorMessage));
            var notifications = new List<DomainNotification>(errors);

            return new Result<ICollection<DomainNotification>>(notifications, "Ocorreu um erro", false);
        }

        public static Result<ICollection<DomainNotification>> CreateNotification(string property, string message)
        {
            var notifications = new List<DomainNotification>()
            {
                new DomainNotification(property, message)
            };

            return new Result<ICollection<DomainNotification>>(notifications, "Ocorreu um erro", false);
        }
    }
}