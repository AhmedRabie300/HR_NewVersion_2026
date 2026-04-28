using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Validators
{
    public class UpdateGradeTransactionValidator : AbstractValidator<UpdateGradeTransactionDto>
    {
        public UpdateGradeTransactionValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.GradeId)
                .GreaterThan(0).WithMessage(x => msg.Get("GradeIdRequired"));

            RuleFor(x => x.TransactionTypeId)
                .GreaterThan(0).WithMessage(x => msg.Get("TransactionTypeRequired"));

            RuleFor(x => x.MinValue)
                .GreaterThanOrEqualTo(0).When(x => x.MinValue.HasValue)
                .WithMessage(x => msg.Get("MinValueMustBePositive"));

            RuleFor(x => x.MaxValue)
                .GreaterThanOrEqualTo(0).When(x => x.MaxValue.HasValue)
                .WithMessage(x => msg.Get("MaxValueMustBePositive"));

            RuleFor(x => x.PaidAtVacation)
                .GreaterThanOrEqualTo(0).When(x => x.PaidAtVacation.HasValue)
                .WithMessage(x => msg.Get("PaidAtVacationMustBePositive"));

            RuleFor(x => x.NumberOfTickets)
                .GreaterThanOrEqualTo(0).When(x => x.NumberOfTickets.HasValue)
                .WithMessage(x => msg.Get("NumberOfTicketsMustBePositive"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateGradeTransactionDto dto)
        {
            return dto.MinValue.HasValue ||
                   dto.MaxValue.HasValue ||
                   dto.PaidAtVacation.HasValue ||
                   dto.OnceAtPeriod.HasValue ||
                   dto.IntervalId.HasValue ||
                   dto.NumberOfTickets.HasValue ||
                   !string.IsNullOrWhiteSpace(dto.Remarks);
        }
    }
}