using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationsPaidType.Dtos;
using FluentValidation;

namespace Application.System.HRS.VacationsPaidType.Validators
{
    public class CreateVacationsPaidTypeValidator : AbstractValidator<CreateVacationsPaidTypeDto>
    {
        private readonly IVacationsPaidTypeRepository _repo;

        public CreateVacationsPaidTypeValidator(IValidationMessages msg, IVacationsPaidTypeRepository repo)
        {
            _repo = repo;

            // ✅ التحقق من وجود Code (NotEmpty)
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (code, cancellation) =>
                {
                    return !await _repo.CodeExistsAsync(code);
                })
                .WithMessage(x => msg.Format("CodeExists", msg.Get("VacationsPaidType"), x.Code));

            RuleFor(x => x.EngName)
                .NotEmpty().WithMessage(x => msg.Get("EngNameRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (engName, cancellation) =>
                {
                    return await _repo.IsEngNameUniqueAsync(engName, null, cancellation);
                })
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .NotEmpty().WithMessage(x => msg.Get("ArbNameRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (arbName, cancellation) =>
                {
                    return await _repo.IsArbNameUniqueAsync(arbName, null, cancellation);
                })
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));
        }
    }
}