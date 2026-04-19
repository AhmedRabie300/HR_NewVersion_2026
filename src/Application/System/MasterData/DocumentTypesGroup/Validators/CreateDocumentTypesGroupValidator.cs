using Application.Common.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.DocumentTypesGroup.Validators
{
    public class CreateDocumentTypesGroupValidator : AbstractValidator<CreateDocumentTypesGroupDto>
    {
        public CreateDocumentTypesGroupValidator(IValidationMessages msg)
        {
            // ✅ إضافة التحقق من Code
            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(msg.Format("MaxLength", 2048));
        }
    }
}