using Application.System.MasterData.Company.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.Company.Validators
{
    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyDto>
    {
        public UpdateCompanyValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.EmpFirstName)
                .MaximumLength(10).When(x => x.EmpFirstName != null)
                .WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.EmpSecondName)
                .MaximumLength(10).When(x => x.EmpSecondName != null)
                .WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.EmpThirdName)
                .MaximumLength(10).When(x => x.EmpThirdName != null)
                .WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.EmpFourthName)
                .MaximumLength(10).When(x => x.EmpFourthName != null)
                .WithMessage(x => msg.Format("MaxLength", 10));

            RuleFor(x => x.EmpNameSeparator)
                .MaximumLength(1).When(x => x.EmpNameSeparator != null)
                .WithMessage(x => msg.Format("MaxLength", 1));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.DefaultTheme)
                .MaximumLength(50).When(x => x.DefaultTheme != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.SequenceLength)
                .GreaterThan(0).When(x => x.SequenceLength.HasValue)
                .WithMessage(x => msg.Get("SequenceLengthMustBePositive"));

            RuleFor(x => x.Prefix)
                .GreaterThan(0).When(x => x.Prefix.HasValue)
                .WithMessage(x => msg.Get("PrefixMustBePositive"));

            RuleFor(x => x.Separator)
                .MaximumLength(1).When(x => x.Separator != null)
                .WithMessage(x => msg.Format("MaxLength", 1));

            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage(x => msg.Get("PrepareDayRange"));

            RuleFor(x => x.ExecuseRequestHoursallowed)
                .GreaterThan(0).When(x => x.ExecuseRequestHoursallowed.HasValue)
                .WithMessage(x => msg.Get("ExecuseRequestHoursPositive"));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateCompanyDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.IsHigry.HasValue ||
                   dto.IncludeAbsencDays.HasValue ||
                   dto.EmpFirstName != null ||
                   dto.EmpSecondName != null ||
                   dto.EmpThirdName != null ||
                   dto.EmpFourthName != null ||
                   dto.EmpNameSeparator != null ||
                   dto.Remarks != null ||
                   dto.PrepareDay.HasValue ||
                   dto.DefaultTheme != null ||
                   dto.VacationIsAccum.HasValue ||
                   dto.HasSequence.HasValue ||
                   dto.SequenceLength.HasValue ||
                   dto.Prefix.HasValue ||
                   dto.Separator != null ||
                   dto.SalaryCalculation.HasValue ||
                   dto.DefaultAttend.HasValue ||
                   dto.CountEmployeeVacationDaysTotal.HasValue ||
                   dto.ZeroBalAfterVac.HasValue ||
                   dto.VacSettlement.HasValue ||
                   dto.AllowOverVacation.HasValue ||
                   dto.VacationFromPrepareDay.HasValue ||
                   dto.ExecuseRequestHoursallowed.HasValue ||
                   dto.EmployeeDocumentsAutoSerial.HasValue ||
                   dto.UserDepartmentsPermissions.HasValue;
        }
    }
}