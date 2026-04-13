using Application.System.MasterData.MaritalStatus.Dtos;
using Application.Common.Abstractions;
using FluentValidation;

namespace Application.System.MasterData.MaritalStatus.Validators
{
    public class CreateMaritalStatusValidator : AbstractValidator<CreateMaritalStatusDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _ContextService;

        public CreateMaritalStatusValidator(ILocalizationService localizer, IContextService ContextService)
        {
            _localizer = localizer;
            _ContextService = ContextService;

            //RuleFor(x => x.Code)
            //    .NotEmpty().WithMessage(x => _localizer.GetMessage("CodeRequired", _ContextService.GetCurrentLanguage()))
            //    .MaximumLength(50).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => string.Format(_localizer.GetMessage("MaxLength", _ContextService.GetCurrentLanguage()), 2048));
        }
    }
}