using Application.Common.Abstractions;
using Application.UARbac.Modules.Dtos;
using FluentValidation;

namespace Application.UARbac.Modules.Validators
{
    public class UpdateModuleValidator : AbstractValidator<UpdateModuleDto>
    {
        private readonly IContextService _contextService;
        private readonly ILocalizationService _localizer;

        public UpdateModuleValidator(IContextService contextService, ILocalizationService localizer)
        {
            _contextService = contextService;
            _localizer = localizer;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(_localizer.GetMessage("IdGreaterThanZero", lang));

            RuleFor(x => x.EngName)
                .MaximumLength(100)
                    .When(x => x.EngName != null)
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100)
                    .When(x => x.ArbName != null)
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100)
                    .When(x => x.ArbName4S != null)
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Rank)
                .GreaterThanOrEqualTo(0)
                    .When(x => x.Rank.HasValue)
                    .WithMessage(_localizer.GetMessage("RankMustBePositive", lang));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048)
                    .When(x => x.Remarks != null)
                    .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 2048));

            RuleFor(x => x.FormId)
                .GreaterThan(0)
                    .When(x => x.FormId.HasValue)
                    .WithMessage(_localizer.GetMessage("FormIdGreaterThanZero", lang));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(_localizer.GetMessage("AtLeastOneField", lang));
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