using Application.Common.Abstractions;
using Application.System.HRS.TransactionsGroup.Dtos;
using FluentValidation;

namespace Application.System.HRS.TransactionsGroup.Validators
{
    public class CreateTransactionsGroupValidator : AbstractValidator<CreateTransactionsGroupDto>
    {
        private readonly ILocalizationService _localizer;
        private readonly IContextService _contextService;

        public CreateTransactionsGroupValidator(ILocalizationService localizer, IContextService contextService)
        {
            _localizer = localizer;
            _contextService = contextService;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(_localizer.GetMessage("CodeRequired", lang))
                .MaximumLength(50).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(_localizer.GetMessage("MaxLength", lang), 2048));

            RuleFor(x => x.RegUserId)
                .GreaterThan(0).WithMessage(_localizer.GetMessage("RegUserIdRequired", lang));
        }
    }
}