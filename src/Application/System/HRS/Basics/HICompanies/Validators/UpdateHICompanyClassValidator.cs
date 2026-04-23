using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Basics.HICompanies.Dtos;
using FluentValidation;

namespace Application.System.HRS.Basics.HICompanies.Validators
{
    public class UpdateHICompanyClassValidator : AbstractValidator<UpdateHICompanyClassDto>
    {
        public UpdateHICompanyClassValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.HICompanyId)
                .GreaterThan(0).WithMessage(x => msg.Get("HICompanyIdRequired"));

            RuleFor(x => x.EngName)
                .MaximumLength(50).When(x => x.EngName != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.ArbName)
                .MaximumLength(50).When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(50).When(x => x.ArbName4S != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.CompanyAmount)
                .GreaterThanOrEqualTo(0).When(x => x.CompanyAmount.HasValue)
                .WithMessage(x => msg.Get("CompanyAmountMustBePositive"));

            RuleFor(x => x.EmployeeAmount)
                .GreaterThanOrEqualTo(0).When(x => x.EmployeeAmount.HasValue)
                .WithMessage(x => msg.Get("EmployeeAmountMustBePositive"));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateHICompanyClassDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.Remarks != null ||
                   dto.CompanyAmount.HasValue ||
                   dto.EmployeeAmount.HasValue;
        }
    }
}