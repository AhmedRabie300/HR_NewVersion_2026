// Application/UARbac/ModulePermissions/Validators/CreateModulePermissionValidator.cs
using Application.Common.Abstractions;
using Application.UARbac.ModulePermissions.Dtos;
using FluentValidation;

namespace Application.UARbac.ModulePermissions.Validators
{
    public class CreateModulePermissionValidator : AbstractValidator<CreateModulePermissionDto>
    {
        private readonly IContextService _contextService;
        private readonly ILocalizationService _localizer;

        public CreateModulePermissionValidator(IContextService contextService, ILocalizationService localizer)
        {
            _contextService = contextService;
            _localizer = localizer;

            var lang = _contextService.GetCurrentLanguage();

            RuleFor(x => x.ModuleId)
                .GreaterThan(0)
                .WithMessage(_localizer.GetMessage("ModuleIdRequired", lang));

            RuleFor(x => x)
                .Must(x => (x.GroupId.HasValue && !x.UserId.HasValue) ||
                          (!x.GroupId.HasValue && x.UserId.HasValue))
                .WithMessage(_localizer.GetMessage("EitherGroupOrUser", lang));

            RuleFor(x => x.CanView)
                .Equal(true).When(x => x.CanView.HasValue)
                .WithMessage(_localizer.GetMessage("CanViewMustBeTrue", lang));
        }
    }
}