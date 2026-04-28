using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Validators
{
    public class UpdateGradeStepTransactionValidator : AbstractValidator<UpdateGradeStepTransactionDto>
    {
        public UpdateGradeStepTransactionValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.GradeStepId)
                .GreaterThan(0).WithMessage(x => msg.Get("GradeStepIdRequired"));

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).When(x => x.Amount.HasValue)
                .WithMessage(x => msg.Get("AmountMustBePositive"));

            RuleFor(x => x.ActiveDateD)
                .MaximumLength(3).When(x => x.ActiveDateD != null)
                .WithMessage(x => msg.Format("MaxLength", 3));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateGradeStepTransactionDto dto)
        {
            return dto.Amount.HasValue ||
                   dto.Remarks != null ||
                   dto.Active.HasValue ||
                   dto.ActiveDate.HasValue ||
                   dto.ActiveDateD != null;
        }
    }
}