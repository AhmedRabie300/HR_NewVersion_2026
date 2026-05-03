using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Employees.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.Employees.Validators
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
    {
        private readonly IEmployeeRepository _repo;

        public UpdateEmployeeValidator(IValidationMessages msg, IEmployeeRepository repo)
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
                .WithMessage(x => msg.Format("CodeExists", msg.Get("Employee"), x.Code));

            RuleFor(x => x.SSnNo)
                .MaximumLength(20).When(x => x.SSnNo != null)
                .WithMessage(x => msg.Format("MaxLength", 20))
                .MustAsync(async (dto, ssnNo, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(ssnNo)) return true;
                    return !await _repo.SSnNoExistsAsync(ssnNo.Trim(), dto.Id);
                })
                .When(x => x.SSnNo != null)
                .WithMessage(x => msg.Get("SSNAlreadyExists"));

            // Names
            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            // Contact Info
            RuleFor(x => x.Email)
                .MaximumLength(255).When(x => x.Email != null)
                .WithMessage(x => msg.Format("MaxLength", 255))
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage(x => msg.Get("InvalidEmail"));

            RuleFor(x => x.WorkEmail)
                .MaximumLength(255).When(x => x.WorkEmail != null)
                .WithMessage(x => msg.Format("MaxLength", 255))
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.WorkEmail))
                .WithMessage(x => msg.Get("InvalidEmail"));

            RuleFor(x => x.Phone)
                .MaximumLength(100).When(x => x.Phone != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Mobile)
                .MaximumLength(100).When(x => x.Mobile != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            // Financial
            RuleFor(x => x.WHours)
                .GreaterThan(0).When(x => x.WHours.HasValue)
                .WithMessage(x => msg.Get("WHoursPositive"));

            RuleFor(x => x.MaxLoanDedution)
                .InclusiveBetween(0, 100).When(x => x.MaxLoanDedution.HasValue)
                .WithMessage(x => msg.Get("PercentageBetween0And100"));

            // Dates
            RuleFor(x => x.BirthDate)
                .LessThan(DateTime.Now).When(x => x.BirthDate.HasValue)
                .WithMessage(x => msg.Get("BirthDateMustBeInPast"));

            RuleFor(x => x.JoinDate)
                .LessThanOrEqualTo(DateTime.Now).When(x => x.JoinDate.HasValue)
                .WithMessage(x => msg.Get("JoinDateMustBeInPast"));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateEmployeeDto dto)
        {
            return dto.Code != null ||
                   dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.FamilyEngName != null ||
                   dto.FamilyArbName != null ||
                   dto.FamilyArbName4S != null ||
                   dto.FatherEngName != null ||
                   dto.FatherArbName != null ||
                   dto.FatherArbName4S != null ||
                   dto.GrandEngName != null ||
                   dto.GrandArbName != null ||
                   dto.GrandArbName4S != null ||
                   dto.BirthDate.HasValue ||
                   dto.BirthCityId.HasValue ||
                   dto.ReligionId.HasValue ||
                   dto.MaritalStatusId.HasValue ||
                   dto.Sex != null ||
                   dto.BloodGroupId.HasValue ||
                   dto.BankId.HasValue ||
                   dto.NationalityId.HasValue ||
                   dto.BankAccountNumber != null ||
                   dto.BankAccNumber != null ||
                   dto.DepartmentId.HasValue ||
                   dto.GOSINumber != null ||
                   dto.GOSIJoinDate.HasValue ||
                   dto.GOSIExcludeDate.HasValue ||
                   dto.JoinDate.HasValue ||
                   dto.ExcludeDate.HasValue ||
                   dto.Remarks != null ||
                   dto.BranchId.HasValue ||
                   dto.SponsorId.HasValue ||
                   dto.Email != null ||
                   dto.Phone != null ||
                   dto.Mobile != null ||
                   dto.ManagerCode != null ||
                   dto.MachineCode != null ||
                   dto.SectorId.HasValue ||
                   dto.SSnNo != null ||
                   dto.PassPortNo != null ||
                   dto.EntryNo != null ||
                   dto.Cost1.HasValue ||
                   dto.Cost2.HasValue ||
                   dto.Cost3.HasValue ||
                   dto.Cost4.HasValue ||
                   dto.LaborOfficeNo != null ||
                   dto.LocationId.HasValue ||
                   dto.WHours.HasValue ||
                   dto.IsProjectRelated.HasValue ||
                   dto.IsSpecialForce.HasValue ||
                   dto.MaxLoanDedution.HasValue ||
                   dto.LedgerCode != null ||
                   dto.HasTaqat.HasValue ||
                   dto.BankAccountType != null ||
                   dto.Hasflexiblesalarydist.HasValue ||
                   dto.Paymenttype.HasValue ||
                   dto.WorkEmail != null ||
                   dto.SSNOIssueDate != null ||
                   dto.SSNOExpireDate != null ||
                   dto.PassportIssueDate != null ||
                   dto.PassportExpireDate != null ||
                   dto.AddressAsPerContract != null ||
                   dto.InsertRequestsForAnotherEmployee.HasValue ||
                   dto.IsSocialInsuranceIncluded.HasValue;
        }
    }
}