using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Validators
{
    public class UpdateGradeValidator : AbstractValidator<UpdateGradeDto>
    {
        private readonly IGradeRepository _repo;

        public UpdateGradeValidator(IValidationMessages msg, IGradeRepository repo)
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
                    return !await _repo.CodeExistsAsync(code.Trim(), dto.Id);
                })
                .When(x => x.Code != null)
                .WithMessage(x => msg.Format("CodeExists", msg.Get("Grade"), x.Code));

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

            RuleFor(x => x.FromSalary)
                .GreaterThanOrEqualTo(0).When(x => x.FromSalary.HasValue)
                .WithMessage(x => msg.Get("FromSalaryMustBePositive"));

            RuleFor(x => x.ToSalary)
                .GreaterThanOrEqualTo(0).When(x => x.ToSalary.HasValue)
                .WithMessage(x => msg.Get("ToSalaryMustBePositive"));

            RuleFor(x => x.RegularHours)
                .GreaterThanOrEqualTo(0).When(x => x.RegularHours.HasValue)
                .WithMessage(x => msg.Get("RegularHoursMustBePositive"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));
        }
    }
}