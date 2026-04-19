using Application.Common.Abstractions;
using Application.UARbac.FormControls.Dtos;
using FluentValidation;

namespace Application.UARbac.FormControls.Validators;

public sealed class UpdateFormControlValidator : AbstractValidator<UpdateFormControlDto>
{
    public UpdateFormControlValidator(IValidationMessages msg)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(msg.Get("IdGreaterThanZero"));

        RuleFor(x => x.EngCaption)
            .MaximumLength(100)
            .WithMessage(msg.Format("MaxLength", 100));

        RuleFor(x => x.ArbCaption)
            .MaximumLength(100)
            .WithMessage(msg.Format("MaxLength", 100));

        RuleFor(x => x)
            .Must(x =>
                x.EngCaption != null ||
                x.ArbCaption != null ||
                x.IsHide.HasValue ||
                x.IsDisabled.HasValue)
            .WithMessage(msg.Get("AtLeastOneField"));
    }
}
