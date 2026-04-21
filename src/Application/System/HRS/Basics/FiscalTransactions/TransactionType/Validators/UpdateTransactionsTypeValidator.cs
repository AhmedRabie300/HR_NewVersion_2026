using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.TransactionType.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.FiscalTransactions.TransactionType.Validators
{
    public class UpdateTransactionsTypeValidator : AbstractValidator<UpdateTransactionsTypeDto>
    {
        private readonly ITransactionsTypeRepository _repo;
        private readonly ITransactionsGroupRepository _transactionsGroupRepo;

        public UpdateTransactionsTypeValidator(
            IValidationMessages msg,
            ITransactionsTypeRepository repo,
            ITransactionsGroupRepository transactionsGroupRepo)
        {
            _repo = repo;
            _transactionsGroupRepo = transactionsGroupRepo;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.Code)
                .MaximumLength(50).When(x => x.Code != null)
                .WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (dto, code, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(code)) return true;
                    return !await _repo.CodeExistsAsync(code.Trim(), dto.Id);
                })
                .When(x => x.Code != null)
                .WithMessage(x => msg.Format("CodeExists", msg.Get("TransactionsType"), x.Code));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (dto, engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName.Trim(), dto.Id, cancellation);
                })
                .When(x => x.EngName != null)
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (dto, arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName.Trim(), dto.Id, cancellation);
                })
                .When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ShortEngName)
                .MaximumLength(50).When(x => x.ShortEngName != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.ShortArbName)
                .MaximumLength(50).When(x => x.ShortArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.ShortArbName4S)
                .MaximumLength(50).When(x => x.ShortArbName4S != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.TransactionGroupId)
                .GreaterThan(0).When(x => x.TransactionGroupId.HasValue)
                .WithMessage(x => msg.Get("TransactionGroupRequired"))
                .MustAsync(async (groupId, cancellation) =>
                {
                    if (!groupId.HasValue) return true;
                    return await _transactionsGroupRepo.ExistsAsync(groupId.Value);
                })
                .WithMessage(x => msg.Get("TransactionGroupNotFound"));

            RuleFor(x => x.Sign)
                .Must(x => !x.HasValue || x.Value == 1 || x.Value == -1)
                .WithMessage(x => msg.Get("SignMustBeOneOrMinusOne"));

            RuleFor(x => x.DebitAccountCode)
                .MaximumLength(50).When(x => x.DebitAccountCode != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.CreditAccountCode)
                .MaximumLength(50).When(x => x.CreditAccountCode != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Formula)
                .MaximumLength(2048).When(x => x.Formula != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.BeginContractFormula)
                .MaximumLength(2048).When(x => x.BeginContractFormula != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.EndContractFormula)
                .MaximumLength(2048).When(x => x.EndContractFormula != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateTransactionsTypeDto dto)
        {
            return dto.Code != null ||
                   dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.ShortEngName != null ||
                   dto.ShortArbName != null ||
                   dto.ShortArbName4S != null ||
                   dto.TransactionGroupId.HasValue ||
                   dto.Sign.HasValue ||
                   dto.DebitAccountCode != null ||
                   dto.CreditAccountCode != null ||
                   dto.IsPaid.HasValue ||
                   dto.Formula != null ||
                   dto.BeginContractFormula != null ||
                   dto.EndContractFormula != null ||
                   dto.InputIsNumeric.HasValue ||
                   dto.IsEndOfService.HasValue ||
                   dto.IsSalaryEOSExeclude.HasValue ||
                   dto.IsProjectRelatedItem.HasValue ||
                   dto.IsBasicSalary.HasValue ||
                   dto.IsDistributable.HasValue ||
                   dto.IsAllowPosting.HasValue ||
                   dto.Remarks != null ||
                   dto.HasInsuranceTiers.HasValue;
        }
    }
}