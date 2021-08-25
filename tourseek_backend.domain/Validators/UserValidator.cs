using FluentValidation;
using tourseek_backend.domain.DTO.UserDTOs;

namespace tourseek_backend.domain.Validators
{
    public class UserValidator : AbstractValidator<CreateUserDto>
    {
        public UserValidator()
        {
            RuleFor(user => user.Password)
               .Length(6, 255)
               .WithMessage("Please Enter a valid passwod with min Lenght 6 and max 255.");

            RuleFor(user => user.Email)
              .EmailAddress()
              .WithMessage("Please Enter a valid Email.");

            RuleFor(user => user.UserName)
               .Length(3, 20)
               .NotEmpty()
               .NotNull()
               .WithMessage("Please Enter a valid userName.");

            RuleFor(user => user.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .Length(9, 11)
                .Matches(@"(0)[0-9]{9}")
                .WithMessage("Please Enter a valid Phone number.");
        }
    }
}
