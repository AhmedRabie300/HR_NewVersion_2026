using Application.Common.Abstractions;
using Application.System.MasterData.Education.Dtos;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.Education.Validators
{
    public class CreateEducationValidator : AbstractValidator<CreateEducationDto>
    {
        public CreateEducationValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.Level)
                .InclusiveBetween(1, 10).When(x => x.Level.HasValue)
                .WithMessage(msg.Get("EducationLevelRange"));

            RuleFor(x => x.RequiredYears)
                .InclusiveBetween(0, 20).When(x => x.RequiredYears.HasValue)
                .WithMessage(msg.Get("RequiredYearsRange"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(msg.Format("MaxLength", 2048));
        }
    }
}