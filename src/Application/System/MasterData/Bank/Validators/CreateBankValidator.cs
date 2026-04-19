using Application.Common.Abstractions;
using Application.System.MasterData.Bank.Dtos;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.Bank.Validators
{
    public class CreateBankValidator : AbstractValidator<CreateBankDto>
    {
        public CreateBankValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(string.Format(msg.Get("MaxLength"), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(msg.Get("MaxLength"), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(msg.Get("MaxLength"), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(msg.Get("MaxLength"), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(msg.Get("MaxLength"), 2048));
        }
    }
}