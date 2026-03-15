using Application.UARbac.Modules.Dtos;
using FluentValidation;

namespace Application.UARbac.Modules.Validators
{
    public class CreateModuleValidator : AbstractValidator<CreateModuleDto>
    {
        public CreateModuleValidator()
        {
             RuleFor(x => x.Code)
                .NotEmpty()
                    .WithMessage("Module code is required")
                .MaximumLength(50)
                    .WithMessage("Module code must not exceed 50 characters")
                .Matches("^[A-Za-z0-9_]+$")
                    .WithMessage("Module code can only contain letters, numbers and underscore");

             RuleFor(x => x.Prefix)
                .MaximumLength(30)
                    .When(x => !string.IsNullOrEmpty(x.Prefix))
                    .WithMessage("Prefix must not exceed 30 characters");

             RuleFor(x => x.EngName)
                .MaximumLength(100)
                    .When(x => !string.IsNullOrEmpty(x.EngName))
                    .WithMessage("English name must not exceed 100 characters");

             RuleFor(x => x.ArbName)
                .MaximumLength(100)
                    .When(x => !string.IsNullOrEmpty(x.ArbName))
                    .WithMessage("Arabic name must not exceed 100 characters");

             RuleFor(x => x.ArbName4S)
                .MaximumLength(100)
                    .When(x => !string.IsNullOrEmpty(x.ArbName4S))
                    .WithMessage("Arabic name (4S) must not exceed 100 characters");

             RuleFor(x => x.FormId)
                .GreaterThan(0)
                    .When(x => x.FormId.HasValue)
                    .WithMessage("Form ID must be greater than 0");

             RuleFor(x => x.Rank)
                .GreaterThanOrEqualTo(0)
                    .When(x => x.Rank.HasValue)
                    .WithMessage("Rank must be zero or positive number");

             RuleFor(x => x.Remarks)
                .MaximumLength(2048)
                    .When(x => !string.IsNullOrEmpty(x.Remarks))
                    .WithMessage("Remarks must not exceed 2048 characters");
             
        }
    }
}