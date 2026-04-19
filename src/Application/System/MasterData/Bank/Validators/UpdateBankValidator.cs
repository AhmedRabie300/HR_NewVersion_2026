using Application.Common.Abstractions;
using Application.System.MasterData.Bank.Dtos;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.Bank.Validators
{
    public class UpdateBankValidator : AbstractValidator<UpdateBankDto>
    {

        public UpdateBankValidator(IValidationMessages msg)
        {

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(string.Format(msg.Format("MaxLength"), 100));
            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(string.Format(msg.Format("MaxLength"), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(string.Format(msg.Format("MaxLength"), 100));
            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(string.Format(msg.Format("MaxLength"), 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateBankDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.Remarks != null;
        }
    }
}