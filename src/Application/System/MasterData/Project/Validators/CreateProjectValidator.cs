using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Project.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Project.Validators
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
    {
        private readonly IProjectRepository _repo;

        public CreateProjectValidator(IValidationMessages msg, IProjectRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (code, cancellation) =>
                {
                    return !await _repo.CodeExistsAsync(code);
                })
                .WithMessage(x => msg.Format("CodeExists", msg.Get("Project"), x.Code));

            RuleFor(x => x.EngName)
                .NotEmpty().WithMessage(x => msg.Get("EngNameRequired"))
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (engName, cancellation) =>
                {
                    return await _repo.IsEngNameUniqueAsync(engName, null, cancellation);
                })
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .NotEmpty().WithMessage(x => msg.Get("ArbNameRequired"))
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (arbName, cancellation) =>
                {
                    return await _repo.IsArbNameUniqueAsync(arbName, null, cancellation);
                })
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Phone)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Mobile)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Fax)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Email)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage(x => msg.Get("InvalidEmail"));

            RuleFor(x => x.Adress)
                .MaximumLength(1024).WithMessage(x => msg.Format("MaxLength", 1024));

            RuleFor(x => x.ContactPerson)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

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
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode2)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode3)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode4)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.WorkConditions)
                .MaximumLength(8000).WithMessage(x => msg.Format("MaxLength", 8000));

            RuleFor(x => x.CompanyConditions)
                .MaximumLength(8000).WithMessage(x => msg.Format("MaxLength", 8000));

            RuleFor(x => x.ClientConditions)
                .MaximumLength(8000).WithMessage(x => msg.Format("MaxLength", 8000));
        }
    }
}