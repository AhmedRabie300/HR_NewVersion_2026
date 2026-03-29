using Application.System.MasterData.Company.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Company.Validators
{
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyDto>
    {
        public CreateCompanyValidator(ILocalizationService localizer, int lang)
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(localizer.GetMessage("CodeRequired", lang))
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.EmpFirstName)
                .MaximumLength(10).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.EmpSecondName)
                .MaximumLength(10).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.EmpThirdName)
                .MaximumLength(10).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.EmpFourthName)
                .MaximumLength(10).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));

            RuleFor(x => x.DefaultTheme)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.SequenceLength)
                .GreaterThan(0).When(x => x.SequenceLength.HasValue)
                .WithMessage(localizer.GetMessage("SequenceLengthMustBePositive", lang));

            RuleFor(x => x.Prefix)
                .GreaterThan(0).When(x => x.Prefix.HasValue)
                .WithMessage(localizer.GetMessage("PrefixMustBePositive", lang));

            RuleFor(x => x.Separator)
                .MaximumLength(1).When(x => !string.IsNullOrEmpty(x.Separator))
                .WithMessage(localizer.GetMessage("SeparatorMaxLength", lang));

            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage(localizer.GetMessage("PrepareDayRange", lang));

            RuleFor(x => x.ExecuseRequestHoursallowed)
                .GreaterThan(0).When(x => x.ExecuseRequestHoursallowed.HasValue)
                .WithMessage(localizer.GetMessage("ExecuseRequestHoursPositive", lang));
        }
    }
}