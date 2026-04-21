using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.VacationsType.Dtos;
using FluentValidation;

namespace Application.System.HRS.VacationAndEndOfService.VacationsType.Validators
{
    public class CreateVacationsTypeValidator : AbstractValidator<CreateVacationsTypeDto>
    {
        private readonly IVacationsTypeRepository _repo;

        public CreateVacationsTypeValidator(IValidationMessages msg, IVacationsTypeRepository repo)
        {
            _repo = repo;
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))   
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (code, cancellation) =>
                {
                    return !await _repo.CodeExistsAsync(code);
                })
                .WithMessage(x => msg.Format("CodeExists", msg.Get("VacationsType"), x.Code));

            RuleFor(x => x.EngName)
                .NotEmpty().WithMessage(x => msg.Get("EngNameRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (engName, cancellation) =>
                {
                    return await _repo.IsEngNameUniqueAsync(engName,null, cancellation);
                })
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .NotEmpty().WithMessage(x => msg.Get("ArbNameRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (arbName, cancellation) =>
                {
                    return await _repo.IsArbNameUniqueAsync(arbName, null,cancellation);
                })
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Sex)
                .MaximumLength(1).WithMessage(x => msg.Format("MaxLength", 1));

            RuleFor(x => x.Religion)
                .MaximumLength(10).WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.TimesNoInYear)
                .GreaterThan(0).When(x => x.TimesNoInYear.HasValue)
                .WithMessage(x => msg.Get("TimesNoInYearGreaterThanZero"));

            RuleFor(x => x.AllowedDaysNo)
                .GreaterThan(0).When(x => x.AllowedDaysNo.HasValue)
                .WithMessage(x => msg.Get("AllowedDaysNoGreaterThanZero"));

            RuleFor(x => x.Stage1Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage1Pct.HasValue)
                .WithMessage(x => msg.Get("PercentageBetween0And100"));

            RuleFor(x => x.Stage2Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage2Pct.HasValue)
                .WithMessage(x => msg.Get("PercentageBetween0And100"));

            RuleFor(x => x.Stage3Pct)
                .InclusiveBetween(0, 100).When(x => x.Stage3Pct.HasValue)
                .WithMessage(x => msg.Get("PercentageBetween0And100"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));
        }
    }
}