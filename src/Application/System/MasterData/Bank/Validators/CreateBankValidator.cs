using Application.Common.Abstractions;
using Application.System.MasterData.Bank.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Bank.Validators
{
    public class CreateBankValidator : AbstractValidator<CreateBankDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _contextService;

        public CreateBankValidator(ILocalizationService localizer, IContextService contextService)
        {
            _localizer = localizer;
            _contextService = contextService;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}