using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.Items.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.Items.Validators
{
    public class CreateItemValidator : AbstractValidator<CreateItemDto>
    {
        private readonly IItemRepository _repo;

        public CreateItemValidator(IValidationMessages msg, IItemRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (code, cancellation) =>
                {
                    return !await _repo.CodeExistsAsync(code);
                })
                .WithMessage(x => msg.Format("CodeExists", msg.Get("Item"), x.Code));

            RuleFor(x => x.EngName)
                .MaximumLength(500).WithMessage(x => msg.Format("MaxLength", 500))
                .MustAsync(async (engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .MaximumLength(500).WithMessage(x => msg.Format("MaxLength", 500))
                .MustAsync(async (arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(500).WithMessage(x => msg.Format("MaxLength", 500));

            RuleFor(x => x.PurchasePrice)
                .GreaterThanOrEqualTo(0).When(x => x.PurchasePrice.HasValue)
                .WithMessage(x => msg.Get("PurchasePriceMustBePositive"));

            RuleFor(x => x.LicenseNumber)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.EngName) || !string.IsNullOrWhiteSpace(x.ArbName))
                .WithMessage(x => msg.Get("AtLeastOneNameRequired"));
        }
    }
}