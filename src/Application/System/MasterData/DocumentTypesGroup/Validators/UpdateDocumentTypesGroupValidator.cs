using Application.System.MasterData.DocumentTypesGroup.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.DocumentTypesGroup.Validators
{
    public class UpdateDocumentTypesGroupValidator : AbstractValidator<UpdateDocumentTypesGroupDto>
    {
        public UpdateDocumentTypesGroupValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateDocumentTypesGroupDto dto)
        {
            return dto.EngName != null || dto.ArbName != null || dto.Remarks != null;
        }
    }
}