using Application.System.MasterData.Branch.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Branch.Validators
{
    public class UpdateBranchValidator : AbstractValidator<UpdateBranchDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _ContextService;

        public UpdateBranchValidator(ILocalizationService localizer, IContextService ContextService)
        {
            _localizer = localizer;
            _ContextService = ContextService;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ParentId)
                .GreaterThan(0).When(x => x.ParentId.HasValue)
                .WithMessage(x => _localizer.GetMessage("ParentBranchRequired", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.CountryId)
                .GreaterThan(0).When(x => x.CountryId.HasValue)
                .WithMessage(x => _localizer.GetMessage("CountryRequired", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.CityId)
                .GreaterThan(0).When(x => x.CityId.HasValue)
                .WithMessage(x => _localizer.GetMessage("CityRequired", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage(x => _localizer.GetMessage("PrepareDayRange", _ContextService.GetCurrentLanguage()));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => _localizer.GetMessage("AtLeastOneField", _ContextService.GetCurrentLanguage()));
        }

        private bool HaveAtLeastOneField(UpdateBranchDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.ParentId.HasValue ||
                   dto.CountryId.HasValue ||
                   dto.CityId.HasValue ||
                   dto.DefaultAbsent.HasValue ||
                   dto.PrepareDay.HasValue ||
                   dto.AffectPeriod.HasValue ||
                   dto.Remarks != null;
        }
    }
}