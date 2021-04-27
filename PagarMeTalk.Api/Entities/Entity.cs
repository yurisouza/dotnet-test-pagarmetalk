using FluentValidation;
using FluentValidation.Results;

namespace PagarMeTalk.Api.Entities
{
    public abstract class Entity<T> : AbstractValidator<T>
        where T : Entity<T>
    {
        public ValidationResult ValidationResult { get; set; }

        public Entity()
        {
            ValidationResult = new ValidationResult();
        }

        public abstract bool IsValid();

        protected void AddErrors(ValidationResult validateResult)
        {
            foreach (var error in validateResult.Errors)
            {
                ValidationResult.Errors.Add(error);
            }
        }

        protected void AddError(string propertyName, string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(propertyName, message));
        }
    }
}