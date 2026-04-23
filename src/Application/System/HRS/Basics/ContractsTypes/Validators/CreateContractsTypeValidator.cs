using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.ContractsTypes.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.ContractsTypes.Validators
{
    public class CreateContractsTypeValidator : AbstractValidator<CreateContractsTypeDto>
    {
        private readonly IContractsTypeRepository _repo;

        public CreateContractsTypeValidator(IValidationMessages msg, IContractsTypeRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (code, cancellation) => !await _repo.CodeExistsAsync(code))
                .WithMessage(x => msg.Format("CodeExists", msg.Get("ContractsType"), x.Code));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.EngName) || !string.IsNullOrWhiteSpace(x.ArbName))
                .WithMessage(x => msg.Get("AtLeastOneNameRequired"));
        }
    }
}