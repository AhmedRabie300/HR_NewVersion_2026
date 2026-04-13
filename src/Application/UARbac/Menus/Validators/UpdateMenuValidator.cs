using Application.Common.Abstractions;
using Application.UARbac.Menus.Dtos;
using FluentValidation;

namespace Application.UARbac.Menus.Validators
{
    public class UpdateMenuValidator : AbstractValidator<UpdateMenuDto>
    {
        private readonly IContextService _contextService;
        private readonly ILocalizationService _localizer;

        public UpdateMenuValidator(IContextService contextService, ILocalizationService localizer)
        {
            _contextService = contextService;
            _localizer = localizer;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(_localizer.GetMessage("IdGreaterThanZero", lang));

            RuleFor(x => x.EngName)
                .MaximumLength(200)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 200));

            RuleFor(x => x.ArbName)
                .MaximumLength(200)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 200));

            RuleFor(x => x.Shortcut)
                .MaximumLength(50)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.Image)
                .MaximumLength(500)
                .WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 500));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(_localizer.GetMessage("AtLeastOneField", lang));
        }

        private bool HaveAtLeastOneField(UpdateMenuDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.Shortcut != null ||
                   dto.Rank != null ||
                   dto.ParentId != null ||
                   dto.FormId != null ||
                   dto.ObjectId != null ||
                   dto.ViewFormId != null ||
                   dto.IsHide != null ||
                   dto.Image != null ||
                   dto.ViewType != null;
        }
    }
}