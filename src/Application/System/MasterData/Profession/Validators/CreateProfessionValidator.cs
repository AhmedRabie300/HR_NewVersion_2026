using Application.Common.Abstractions;
using Application.System.MasterData.Profession.Dtos;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.Profession.Validators
{
    public class CreateProfessionValidator : AbstractValidator<CreateProfessionDto>
    {
        public CreateProfessionValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(msg.Format("MaxLength", 2048));
        }
    }
}