using Application.Common.Abstractions;
using Application.UARbac.Modules.Dtos;
using FluentValidation;

namespace Application.UARbac.Modules.Validators
{
    public class UpdateModuleValidator : AbstractValidator<UpdateModuleDto>
    {
        public UpdateModuleValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.EngName)
                .MaximumLength(100)
                .When(x => x.EngName != null)
                .WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100)
                .When(x => x.ArbName != null)
                .WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100)
                .When(x => x.ArbName4S != null)
                .WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.Rank)
                .GreaterThanOrEqualTo(0)
                .When(x => x.Rank.HasValue)
                .WithMessage(msg.Get("RankMustBePositive"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048)
                .When(x => x.Remarks != null)
                .WithMessage(msg.Format("MaxLength", 2048));

            RuleFor(x => x.FormId)
                .GreaterThan(0)
                .When(x => x.FormId.HasValue)
                .WithMessage(msg.Get("FormIdGreaterThanZero"));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(msg.Get("AtLeastOneField"));
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
