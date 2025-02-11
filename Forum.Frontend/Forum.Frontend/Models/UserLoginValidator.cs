using FluentValidation;
using Forum.API.DTO.Users;

namespace Forum.Frontend.Models
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Login)
                .MinimumLength(4)
                .NotEmpty();

            RuleFor(x => x.Password)
                .MinimumLength(5)
                .NotEmpty();
        }
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<UserLoginDto>.CreateWithOptions((UserLoginDto)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
