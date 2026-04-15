using Application.Common.Abstractions;
using Application.System.MasterData.ContractType.Dtos;
using FluentValidation;

namespace Application.System.MasterData.ContractType.Validators
{
    public class CreateContractTypeValidator : AbstractValidator<CreateContractTypeDto>
    {
        public CreateContractTypeValidator(ILocalizationService localizer, IContextService contextService)
        {
            var lang = contextService.GetCurrentLanguage();

            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 50));

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 100));

            RuleFor(x => x.IsSpecial)
                .NotNull().WithMessage(localizer.GetMessage("IsSpecialRequired", lang));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(localizer.GetMessage("MaxLength", lang), 2048));
        }
    }
}