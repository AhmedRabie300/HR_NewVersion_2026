using Application.Common.Abstractions;
using Application.UARbac.Modules.Dtos;
using FluentValidation;

namespace Application.UARbac.Modules.Validators
{
    public class CreateModuleValidator : AbstractValidator<CreateModuleDto>
    {
        public CreateModuleValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(msg.Get("CodeRequired"))
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50))
                .Matches("^[A-Za-z0-9_]+$").WithMessage(msg.Get("CodePattern"));

            RuleFor(x => x.Prefix)
                .MaximumLength(30)
                .When(x => !string.IsNullOrEmpty(x.Prefix))
                .WithMessage(msg.Format("MaxLength", 30));

            RuleFor(x => x.EngName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.EngName))
                .WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.ArbName))
                .WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.ArbName4S))
                .WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.FormId)
                .GreaterThan(0)
                .When(x => x.FormId.HasValue)
                .WithMessage(msg.Get("FormIdGreaterThanZero"));

            RuleFor(x => x.Rank)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Rank.HasValue)
                .WithMessage(msg.Get("RankMustBePositive"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048)
                .When(x => !string.IsNullOrEmpty(x.Remarks))
                .WithMessage(msg.Format("MaxLength", 2048));
        }
    }
}
