using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Project.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Project.Validators
{
    public class UpdateProjectValidator : AbstractValidator<UpdateProjectDto>
    {
        private readonly IProjectRepository _repo;

        public UpdateProjectValidator(IValidationMessages msg, IProjectRepository repo)
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
                    return !await _repo.CodeExistsAsync(code, dto.Id);
                })
                .When(x => x.Code != null)
                .WithMessage(x => msg.Format("CodeExists", msg.Get("Project"), x.Code));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (dto, engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName, dto.Id, cancellation);
                })
                .When(x => x.EngName != null)
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (dto, arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName, dto.Id, cancellation);
                })
                .When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Phone)
                .MaximumLength(50).When(x => x.Phone != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Mobile)
                .MaximumLength(50).When(x => x.Mobile != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Fax)
                .MaximumLength(50).When(x => x.Fax != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Email)
                .MaximumLength(100).When(x => x.Email != null)
                .WithMessage(x => msg.Format("MaxLength", 100))
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage(x => msg.Get("InvalidEmail"));

            RuleFor(x => x.Adress)
                .MaximumLength(1024).When(x => x.Adress != null)
                .WithMessage(x => msg.Format("MaxLength", 1024));

            RuleFor(x => x.ContactPerson)
                .MaximumLength(100).When(x => x.ContactPerson != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ProjectPeriod)
                .GreaterThan(0).When(x => x.ProjectPeriod.HasValue)
                .WithMessage(x => msg.Get("ProjectPeriodMustBePositive"));

            RuleFor(x => x.ClaimDuration)
                .GreaterThan(0).When(x => x.ClaimDuration.HasValue)
                .WithMessage(x => msg.Get("ClaimDurationMustBePositive"));

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate).When(x => x.StartDate.HasValue && x.EndDate.HasValue)
                .WithMessage(x => msg.Get("StartDateMustBeBeforeEndDate"));

            RuleFor(x => x.CreditLimit)
                .GreaterThan(0).When(x => x.CreditLimit.HasValue)
                .WithMessage(x => msg.Get("CreditLimitMustBePositive"));

            RuleFor(x => x.CreditPeriod)
                .GreaterThan(0).When(x => x.CreditPeriod.HasValue)
                .WithMessage(x => msg.Get("CreditPeriodMustBePositive"));

            RuleFor(x => x.NotifyPeriod)
                .GreaterThan(0).When(x => x.NotifyPeriod.HasValue)
                .WithMessage(x => msg.Get("NotifyPeriodMustBePositive"));

            RuleFor(x => x.BranchId)
                .GreaterThan(0).When(x => x.BranchId.HasValue)
                .WithMessage(x => msg.Get("BranchRequired"));

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).When(x => x.DepartmentId.HasValue)
                .WithMessage(x => msg.Get("DepartmentRequired"));

            RuleFor(x => x.LocationId)
                .GreaterThan(0).When(x => x.LocationId.HasValue)
                .WithMessage(x => msg.Get("LocationRequired"));

            RuleFor(x => x.CostCenterCode1)
                .MaximumLength(50).When(x => x.CostCenterCode1 != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode2)
                .MaximumLength(50).When(x => x.CostCenterCode2 != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode3)
                .MaximumLength(50).When(x => x.CostCenterCode3 != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode4)
                .MaximumLength(50).When(x => x.CostCenterCode4 != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.WorkConditions)
                .MaximumLength(8000).When(x => x.WorkConditions != null)
                .WithMessage(x => msg.Format("MaxLength", 8000));

            RuleFor(x => x.CompanyConditions)
                .MaximumLength(8000).When(x => x.CompanyConditions != null)
                .WithMessage(x => msg.Format("MaxLength", 8000));

            RuleFor(x => x.ClientConditions)
                .MaximumLength(8000).When(x => x.ClientConditions != null)
                .WithMessage(x => msg.Format("MaxLength", 8000));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateProjectDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.Phone != null ||
                   dto.Mobile != null ||
                   dto.Fax != null ||
                   dto.Email != null ||
                   dto.Adress != null ||
                   dto.ContactPerson != null ||
                   dto.ProjectPeriod.HasValue ||
                   dto.ClaimDuration.HasValue ||
                   dto.StartDate.HasValue ||
                   dto.EndDate.HasValue ||
                   dto.CreditLimit.HasValue ||
                   dto.CreditPeriod.HasValue ||
                   dto.IsAdvance.HasValue ||
                   dto.IsHijri.HasValue ||
                   dto.NotifyPeriod.HasValue ||
                   dto.CompanyConditions != null ||
                   dto.ClientConditions != null ||
                   dto.IsLocked.HasValue ||
                   dto.IsStoped.HasValue ||
                   dto.BranchId.HasValue ||
                   dto.Remarks != null ||
                   dto.WorkConditions != null ||
                   dto.LocationId.HasValue ||
                   dto.AbsentTransaction.HasValue ||
                   dto.LeaveTransaction.HasValue ||
                   dto.LateTransaction.HasValue ||
                   dto.SickTransaction.HasValue ||
                   dto.OTTransaction.HasValue ||
                   dto.HOTTransaction.HasValue ||
                   dto.CostCenterCode1 != null ||
                   dto.DepartmentId.HasValue ||
                   dto.CostCenterCode2 != null ||
                   dto.CostCenterCode3 != null ||
                   dto.CostCenterCode4 != null;
        }
    }
}