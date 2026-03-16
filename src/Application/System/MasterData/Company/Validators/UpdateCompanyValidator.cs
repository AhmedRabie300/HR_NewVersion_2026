 using Application.System.MasterData.Company.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Company.Validators
{
    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyDto>
    {
        public UpdateCompanyValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Valid company ID is required");

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage("English name must not exceed 100 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage("Arabic name must not exceed 100 characters");

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage("Arabic name (4S) must not exceed 100 characters");

            RuleFor(x => x.EmpFirstName)
                .MaximumLength(10).When(x => x.EmpFirstName != null)
                .WithMessage("First name must not exceed 10 characters");

            RuleFor(x => x.EmpSecondName)
                .MaximumLength(10).When(x => x.EmpSecondName != null)
                .WithMessage("Second name must not exceed 10 characters");

            RuleFor(x => x.EmpThirdName)
                .MaximumLength(10).When(x => x.EmpThirdName != null)
                .WithMessage("Third name must not exceed 10 characters");

            RuleFor(x => x.EmpFourthName)
                .MaximumLength(10).When(x => x.EmpFourthName != null)
                .WithMessage("Fourth name must not exceed 10 characters");

            
            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage("Remarks must not exceed 2048 characters");

            RuleFor(x => x.DefaultTheme)
                .MaximumLength(50).When(x => x.DefaultTheme != null)
                .WithMessage("Default theme must not exceed 50 characters");

            RuleFor(x => x.SequenceLength)
                .GreaterThan(0).When(x => x.SequenceLength.HasValue)
                .WithMessage("Sequence length must be greater than 0");

            RuleFor(x => x.Prefix)
                .GreaterThan(0).When(x => x.Prefix.HasValue)
                .WithMessage("Prefix must be greater than 0");

            RuleFor(x => x.Separator)
                .MaximumLength(1).When(x => x.Separator != null)
                .WithMessage("Separator must be a single character");

            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage("Prepare day must be between 1 and 31");

            RuleFor(x => x.ExecuseRequestHoursallowed)
                .GreaterThan(0).When(x => x.ExecuseRequestHoursallowed.HasValue)
                .WithMessage("Execuse request hours allowed must be greater than 0");

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage("At least one field must be provided for update");
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