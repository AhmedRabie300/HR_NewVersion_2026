using Application.UARbac.FormControls.Dtos;
using FluentValidation;

namespace Application.UARbac.FormControls.Validators;

public sealed class UpdateFormControlValidator : AbstractValidator<UpdateFormControlDto>
{
    public UpdateFormControlValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id is required.");

        RuleFor(x => x.EngCaption)
            .MaximumLength(100)
            .WithMessage("English Caption max length is 100.");

        RuleFor(x => x.ArbCaption)
            .MaximumLength(100)
            .WithMessage("Arabic Caption max length is 100.");

        // Optional: if your business wants at least one change
        RuleFor(x => x)
            .Must(x =>
                x.EngCaption != null ||
                x.ArbCaption != null ||
                x.IsHide.HasValue ||
                x.IsDisabled.HasValue)
            .WithMessage("At least one field must be provided to update.");
    }
}
