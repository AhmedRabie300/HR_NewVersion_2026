using Application.Common.Abstractions;
using Application.UARbac.Menus.Dtos;
using FluentValidation;

namespace Application.UARbac.Menus.Validators
{
    public class UpdateMenuValidator : AbstractValidator<UpdateMenuDto>
    {
        public UpdateMenuValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.EngName)
                .MaximumLength(200).WithMessage(msg.Format("MaxLength", 200));

            RuleFor(x => x.ArbName)
                .MaximumLength(200).WithMessage(msg.Format("MaxLength", 200));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(200).WithMessage(msg.Format("MaxLength", 200));

            RuleFor(x => x.Shortcut)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.Image)
                .MaximumLength(500).WithMessage(msg.Format("MaxLength", 500));

            RuleFor(x => x.Rank)
                .GreaterThanOrEqualTo(0).When(x => x.Rank.HasValue)
                .WithMessage(msg.Get("RankMustBePositive"));
        }
    }
}
