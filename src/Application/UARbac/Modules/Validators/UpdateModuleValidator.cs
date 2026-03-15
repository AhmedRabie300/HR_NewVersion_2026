using Application.UARbac.Modules.Dtos;
using FluentValidation;

namespace Application.UARbac.Modules.Validators
{
    public class UpdateModuleValidator : AbstractValidator<UpdateModuleDto>
    {
        public UpdateModuleValidator()
        {
             RuleFor(x => x.Id)
                .GreaterThan(0)
                    .WithMessage("Valid module ID is required");

             RuleFor(x => x.EngName)
                .MaximumLength(100)
                    .When(x => x.EngName != null)
                    .WithMessage("English name must not exceed 100 characters");

             RuleFor(x => x.ArbName)
                .MaximumLength(100)
                    .When(x => x.ArbName != null)
                    .WithMessage("Arabic name must not exceed 100 characters");

             RuleFor(x => x.ArbName4S)
                .MaximumLength(100)
                    .When(x => x.ArbName4S != null)
                    .WithMessage("Arabic name (4S) must not exceed 100 characters");

             RuleFor(x => x.Rank)
                .GreaterThanOrEqualTo(0)
                    .When(x => x.Rank.HasValue)
                    .WithMessage("Rank must be zero or positive number");

             RuleFor(x => x.Remarks)
                .MaximumLength(2048)
                    .When(x => x.Remarks != null)
                    .WithMessage("Remarks must not exceed 2048 characters");

             RuleFor(x => x.FormId)
                .GreaterThan(0)
                    .When(x => x.FormId.HasValue)
                    .WithMessage("Form ID must be greater than 0");

             RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage("At least one field must be provided for update");
        }

        private bool HaveAtLeastOneField(UpdateModuleDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.Rank.HasValue ||
                   dto.Remarks != null ||
                   dto.FormId.HasValue ||
                   dto.IsRegistered.HasValue ||
                   dto.FiscalYearDependant.HasValue ||
                   dto.IsAR.HasValue ||
                   dto.IsAP.HasValue ||
                   dto.IsGL.HasValue ||
                   dto.IsFA.HasValue ||
                   dto.IsINV.HasValue ||
                   dto.IsHR.HasValue ||
                   dto.IsMANF.HasValue ||
                   dto.IsSYS.HasValue;
        }
    }
}