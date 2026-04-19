using Application.Common.Abstractions;
using Application.System.MasterData.Document.Dtos;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.Document.Validators
{
    public class CreateDocumentValidator : AbstractValidator<CreateDocumentDto>
    {
        public CreateDocumentValidator(IValidationMessages msg)
        {
            // ✅ إضافة التحقق من Code
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

            RuleFor(x => x.DocumentTypesGroupId)
                .GreaterThan(0).When(x => x.DocumentTypesGroupId.HasValue)
                .WithMessage(msg.Get("DocumentTypesGroupRequired"));
        }
    }
}