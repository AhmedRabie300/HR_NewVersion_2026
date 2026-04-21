using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.TransactionType.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.FiscalTransactions.TransactionType.Validators
{
    public class CreateTransactionsTypeValidator : AbstractValidator<CreateTransactionsTypeDto>
    {
        private readonly ITransactionsTypeRepository _repo;
        private readonly ITransactionsGroupRepository _transactionsGroupRepo;

        public CreateTransactionsTypeValidator(
            IValidationMessages msg,
            ITransactionsTypeRepository repo,
            ITransactionsGroupRepository transactionsGroupRepo)
        {
            _repo = repo;
            _transactionsGroupRepo = transactionsGroupRepo;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (code, cancellation) =>
                {
                    return !await _repo.CodeExistsAsync(code);
                })
                .WithMessage(x => msg.Format("CodeExists", msg.Get("TransactionsType"), x.Code));

            RuleFor(x => x.EngName)
                .NotEmpty().WithMessage(x => msg.Get("EngNameRequired"))
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .NotEmpty().WithMessage(x => msg.Get("ArbNameRequired"))
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName.Trim(), null, cancellation);
                })
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ShortEngName)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.ShortArbName)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.ShortArbName4S)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.TransactionGroupId)
                .GreaterThan(0).WithMessage(x => msg.Get("TransactionGroupRequired"))
                .MustAsync(async (groupId, cancellation) =>
                {
                    return await _transactionsGroupRepo.ExistsAsync(groupId);
                })
                .WithMessage(x => msg.Get("TransactionGroupNotFound"));

            RuleFor(x => x.Sign)
                .Must(x => x == 1 || x == -1)
                .WithMessage(x => msg.Get("SignMustBeOneOrMinusOne"));

            RuleFor(x => x.DebitAccountCode)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.CreditAccountCode)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Formula)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.BeginContractFormula)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.EndContractFormula)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));
        }
    }
}