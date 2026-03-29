using Application.System.MasterData.Document.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.Document.Validators
{
    public class UpdateDocumentValidator : AbstractValidator<UpdateDocumentDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly ILanguageService _languageService;

        public UpdateDocumentValidator(ILocalizationService localizer, ILanguageService languageService)
        {
            _localizer = localizer;
            _languageService = languageService;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _languageService.GetCurrentLanguage()));

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _languageService.GetCurrentLanguage()), 2048));

            RuleFor(x => x.DocumentTypesGroupId)
                .GreaterThan(0).When(x => x.DocumentTypesGroupId.HasValue)
                .WithMessage(x => _localizer.GetMessage("DocumentTypesGroupRequired", _languageService.GetCurrentLanguage()));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => _localizer.GetMessage("AtLeastOneField", _languageService.GetCurrentLanguage()));
        }

        private bool HaveAtLeastOneField(UpdateDocumentDto dto)
        {
            return dto.EngName != null || dto.ArbName != null ||
                   dto.ArbName4S != null || dto.IsForCompany.HasValue ||
                   dto.Remarks != null || dto.DocumentTypesGroupId.HasValue;
        }
    }
}