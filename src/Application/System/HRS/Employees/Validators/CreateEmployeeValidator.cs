using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Employees.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.Employees.Validators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
        private readonly IEmployeeRepository _repo;

        public CreateEmployeeValidator(IValidationMessages msg, IEmployeeRepository repo)
        {
            _repo = repo;

             RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
                .MustAsync(async (code, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(code)) return true;
                    return !await _repo.CodeExistsAsync(code.Trim());
                })
                .When(x => !string.IsNullOrWhiteSpace(x.Code))
                .WithMessage(x => msg.Format("CodeExists", msg.Get("Employee"), x.Code));

             RuleFor(x => x.SSnNo)
                .NotEmpty().WithMessage(x => msg.Get("SSNRequired"))
                .MaximumLength(20).WithMessage(x => msg.Format("MaxLength", 20))
                .MustAsync(async (ssnNo, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(ssnNo)) return true;
                    return !await _repo.SSnNoExistsAsync(ssnNo.Trim());
                })
                .WithMessage(x => msg.Get("SSNAlreadyExists"));

             RuleFor(x => x.EngName)
                .NotEmpty().WithMessage(x => msg.Get("EngNameRequired"))
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .NotEmpty().WithMessage(x => msg.Get("ArbNameRequired"))
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            // Family Names
            RuleFor(x => x.FamilyEngName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.FamilyArbName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.FamilyArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

             RuleFor(x => x.FatherEngName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.FatherArbName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.FatherArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            // Grand Names
            RuleFor(x => x.GrandEngName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.GrandArbName)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.GrandArbName4S)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            // Contact Info
            RuleFor(x => x.Email)
                .MaximumLength(255).WithMessage(x => msg.Format("MaxLength", 255))
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage(x => msg.Get("InvalidEmail"));

            RuleFor(x => x.WorkEmail)
                .MaximumLength(255).WithMessage(x => msg.Format("MaxLength", 255))
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.WorkEmail))
                .WithMessage(x => msg.Get("InvalidEmail"));

            RuleFor(x => x.Phone)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Mobile)
                .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100));

            // Government IDs
            RuleFor(x => x.PassPortNo)
                .MaximumLength(20).WithMessage(x => msg.Format("MaxLength", 20));

            RuleFor(x => x.EntryNo)
                .MaximumLength(20).WithMessage(x => msg.Format("MaxLength", 20));

            RuleFor(x => x.LaborOfficeNo)
                .MaximumLength(30).WithMessage(x => msg.Format("MaxLength", 30));

            RuleFor(x => x.MachineCode)
                .MaximumLength(20).WithMessage(x => msg.Format("MaxLength", 20));

            RuleFor(x => x.LedgerCode)
                .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50));

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

            RuleFor(x => x.GOSIJoinDate)
                .LessThanOrEqualTo(DateTime.Now).When(x => x.GOSIJoinDate.HasValue)
                .WithMessage(x => msg.Get("GOSIJoinDateMustBeInPast"));

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.EngName) || !string.IsNullOrWhiteSpace(x.ArbName))
                .WithMessage(x => msg.Get("AtLeastOneNameRequired"));
        }
    }
}