using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.HICompanies.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.HICompanies.Validators
{
    public class CreateHICompanyValidator : AbstractValidator<CreateHICompanyDto>
    {
        private readonly IHICompanyRepository _repo;

        public CreateHICompanyValidator(IValidationMessages msg, IHICompanyRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (code, cancellation) =>
                {
                    return !await _repo.CodeExistsAsync(code);
                })
                .WithMessage(x => msg.Format("CodeExists", msg.Get("HICompany"), x.Code));

            RuleFor(x => x.EngName)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.Classes)
                .NotNull().WithMessage(x => msg.Get("ClassesRequired"))
                .Must(classes => classes != null && classes.Any())
                .WithMessage(x => msg.Get("AtLeastOneClassRequired"));

            RuleForEach(x => x.Classes)
                .SetValidator(new CreateHICompanyClassValidator(msg));
        }
    }

    public class CreateHICompanyClassValidator : AbstractValidator<CreateHICompanyClassDto>
    {
        public CreateHICompanyClassValidator(IValidationMessages msg)
        {
            RuleFor(x => x.EngName)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.ArbName)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.CompanyAmount)
                .GreaterThanOrEqualTo(0).When(x => x.CompanyAmount.HasValue)
                .WithMessage(x => msg.Get("CompanyAmountMustBePositive"));

            RuleFor(x => x.EmployeeAmount)
                .GreaterThanOrEqualTo(0).When(x => x.EmployeeAmount.HasValue)
                .WithMessage(x => msg.Get("EmployeeAmountMustBePositive"));

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.EngName) || !string.IsNullOrWhiteSpace(x.ArbName))
                .WithMessage(x => msg.Get("AtLeastOneNameRequired"));
        }
    }
}