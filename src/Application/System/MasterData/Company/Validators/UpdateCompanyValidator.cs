using Application.System.MasterData.Company.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Company.Validators
{
    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyDto>
    {
        public UpdateCompanyValidator(ILocalizationService localizer, int lang)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.EmpFirstName)
                .MaximumLength(10).When(x => x.EmpFirstName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.EmpSecondName)
                .MaximumLength(10).When(x => x.EmpSecondName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.EmpThirdName)
                .MaximumLength(10).When(x => x.EmpThirdName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.EmpFourthName)
                .MaximumLength(10).When(x => x.EmpFourthName != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 10));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));

            RuleFor(x => x.DefaultTheme)
                .MaximumLength(50).When(x => x.DefaultTheme != null)
                .WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.SequenceLength)
                .GreaterThan(0).When(x => x.SequenceLength.HasValue)
                .WithMessage(localizer.GetMessage("SequenceLengthMustBePositive", lang));

            RuleFor(x => x.Prefix)
                .GreaterThan(0).When(x => x.Prefix.HasValue)
                .WithMessage(localizer.GetMessage("PrefixMustBePositive", lang));

            RuleFor(x => x.Separator)
                .MaximumLength(1).When(x => x.Separator != null)
                .WithMessage(localizer.GetMessage("SeparatorMaxLength", lang));

            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage(localizer.GetMessage("PrepareDayRange", lang));

            RuleFor(x => x.ExecuseRequestHoursallowed)
                .GreaterThan(0).When(x => x.ExecuseRequestHoursallowed.HasValue)
                .WithMessage(localizer.GetMessage("ExecuseRequestHoursPositive", lang));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(localizer.GetMessage("AtLeastOneField", lang));
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