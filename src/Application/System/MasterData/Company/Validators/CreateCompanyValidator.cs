using Application.System.MasterData.Company.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.Company.Validators
{
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyDto>
    {
        public CreateCompanyValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.EmpFirstName)
                .MaximumLength(10).WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.EmpSecondName)
                .MaximumLength(10).WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.EmpThirdName)
                .MaximumLength(10).WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.EmpFourthName)
                .MaximumLength(10).WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.EmpNameSeparator)
                .MaximumLength(1).When(x => !string.IsNullOrEmpty(x.EmpNameSeparator))
                .WithMessage(x => msg.Format("MaxLength", 1));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.DefaultTheme)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.SequenceLength)
                .GreaterThan(0).When(x => x.SequenceLength.HasValue)
                .WithMessage(x => msg.Get("SequenceLengthMustBePositive"));

            RuleFor(x => x.Prefix)
                .GreaterThan(0).When(x => x.Prefix.HasValue)
                .WithMessage(x => msg.Get("PrefixMustBePositive"));

            RuleFor(x => x.Separator)
                .MaximumLength(1).When(x => !string.IsNullOrEmpty(x.Separator))
                .WithMessage(x => msg.Format("MaxLength", 1));

            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage(x => msg.Get("PrepareDayRange"));

            RuleFor(x => x.ExecuseRequestHoursallowed)
                .GreaterThan(0).When(x => x.ExecuseRequestHoursallowed.HasValue)
                .WithMessage(x => msg.Get("ExecuseRequestHoursPositive"));
        }
    }
}