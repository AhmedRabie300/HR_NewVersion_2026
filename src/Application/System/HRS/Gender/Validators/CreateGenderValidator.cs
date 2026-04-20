using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Gender.Dtos;
using FluentValidation;

namespace Application.System.HRS.Gender.Validators
{
    public class CreateGenderValidator : AbstractValidator<CreateGenderDto>
    {
        private readonly IGenderRepository _repo;

        public CreateGenderValidator(IValidationMessages msg, IGenderRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.EngName)
                .NotEmpty().WithMessage(x => msg.Get("EngNameRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (engName, cancellation) =>
                {
                    return await _repo.IsEngNameUniqueAsync(engName, cancellation);
                })
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .NotEmpty().WithMessage(x => msg.Get("ArbNameRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (arbName, cancellation) =>
                {
                    return await _repo.IsArbNameUniqueAsync(arbName, cancellation);
                })
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));
        }
    }
}