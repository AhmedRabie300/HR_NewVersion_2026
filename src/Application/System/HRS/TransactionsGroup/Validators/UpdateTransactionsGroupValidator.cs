using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.TransactionsGroup.Dtos;
using FluentValidation;

namespace Application.System.HRS.TransactionsGroup.Validators
{
    public class UpdateTransactionsGroupValidator : AbstractValidator<UpdateTransactionsGroupDto>
    {
        private readonly ITransactionsGroupRepository _repo;

        public UpdateTransactionsGroupValidator(IValidationMessages msg, ITransactionsGroupRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

             RuleFor(x => x.Code)
                .MaximumLength(50).When(x => x.Code != null)
                .WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (dto, code, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(code)) return true;
                    return !await _repo.CodeExistsAsync(code, dto.Id);
                })
                .When(x => x.Code != null)
                .WithMessage(x => msg.Format("CodeExists", msg.Get("TransactionsGroup"), x.Code));
            RuleFor(x => x.EngName)
                       .MaximumLength(50).When(x => x.EngName != null)
                       .WithMessage(x => msg.Format("MaxLength", 50))
                       .MustAsync(async (dto, engName, cancellation) =>
                       {
                           if (string.IsNullOrWhiteSpace(engName)) return true;
                           return await _repo.IsEngNameUniqueAsync(engName, dto.Id, cancellation);
                       })
                       .When(x => x.EngName != null)
                       .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .MaximumLength(50).When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (dto, arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName, dto.Id, cancellation);
                })
                .When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateTransactionsGroupDto dto)
        {
            return dto.Code != null ||
                   dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.Remarks != null;
        }
    }
}