using Application.System.MasterData.Company.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Company.Validators
{
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyDto>
    {
        public CreateCompanyValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Company code is required")
                .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage("English name must not exceed 100 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage("Arabic name must not exceed 100 characters");

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage("Arabic name (4S) must not exceed 100 characters");

            RuleFor(x => x.EmpFirstName)
                .MaximumLength(10).WithMessage("First name must not exceed 10 characters");

            RuleFor(x => x.EmpSecondName)
                .MaximumLength(10).WithMessage("Second name must not exceed 10 characters");

            RuleFor(x => x.EmpThirdName)
                .MaximumLength(10).WithMessage("Third name must not exceed 10 characters");

            RuleFor(x => x.EmpFourthName)
                .MaximumLength(10).WithMessage("Fourth name must not exceed 10 characters");

           
            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage("Remarks must not exceed 2048 characters");

            RuleFor(x => x.DefaultTheme)
                .MaximumLength(50).WithMessage("Default theme must not exceed 50 characters");

            RuleFor(x => x.SequenceLength)
                .GreaterThan(0).When(x => x.SequenceLength.HasValue)
                .WithMessage("Sequence length must be greater than 0");

            RuleFor(x => x.Prefix)
                .GreaterThan(0).When(x => x.Prefix.HasValue)
                .WithMessage("Prefix must be greater than 0");

            RuleFor(x => x.Separator)
                .MaximumLength(1).WithMessage("Separator must be a single character");

            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage("Prepare day must be between 1 and 31");

            RuleFor(x => x.ExecuseRequestHoursallowed)
                .GreaterThan(0).When(x => x.ExecuseRequestHoursallowed.HasValue)
                .WithMessage("Execuse request hours allowed must be greater than 0");
        }
    }
}