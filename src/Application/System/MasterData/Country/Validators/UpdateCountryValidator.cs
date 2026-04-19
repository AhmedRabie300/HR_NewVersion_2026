using Application.Common.Abstractions;
using Application.System.MasterData.Country.Dtos;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.Country.Validators
{
    public class UpdateCountryValidator : AbstractValidator<UpdateCountryDto>
    {
        public UpdateCountryValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));

            RuleFor(x => x.EngName)
                .MaximumLength(255).When(x => x.EngName != null)
                .WithMessage(msg.Format("MaxLength", 255));

            RuleFor(x => x.ArbName)
                .MaximumLength(255).When(x => x.ArbName != null)
                .WithMessage(msg.Format("MaxLength", 255));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(255).When(x => x.ArbName4S != null)
                .WithMessage(msg.Format("MaxLength", 255));

            RuleFor(x => x.PhoneKey)
                .MaximumLength(50).When(x => x.PhoneKey != null)
                .WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.ISOAlpha2)
                .MaximumLength(2).When(x => x.ISOAlpha2 != null)
                .WithMessage(msg.Format("MaxLength", 2));

            RuleFor(x => x.ISOAlpha3)
                .MaximumLength(3).When(x => x.ISOAlpha3 != null)
                .WithMessage(msg.Format("MaxLength", 3));

            RuleFor(x => x.Languages)
                .MaximumLength(100).When(x => x.Languages != null)
                .WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.Continent)
                .MaximumLength(10).When(x => x.Continent != null)
                .WithMessage(msg.Format("MaxLength", 10));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateCountryDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.CurrencyId.HasValue ||
                   dto.NationalityId.HasValue ||
                   dto.PhoneKey != null ||
                   dto.IsMainCountries.HasValue ||
                   dto.Remarks != null ||
                   dto.RegionId.HasValue ||
                   dto.ISOAlpha2 != null ||
                   dto.ISOAlpha3 != null ||
                   dto.Languages != null ||
                   dto.Continent != null ||
                   dto.CapitalId.HasValue;
        }
    }
}