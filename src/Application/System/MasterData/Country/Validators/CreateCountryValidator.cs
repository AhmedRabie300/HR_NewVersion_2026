using Application.Common.Abstractions;
using Application.System.MasterData.Country.Dtos;
using FluentValidation;
using Application.Abstractions;

namespace Application.System.MasterData.Country.Validators
{
    public class CreateCountryValidator : AbstractValidator<CreateCountryDto>
    {
        public CreateCountryValidator(IValidationMessages msg)
        {
            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.EngName)
                .MaximumLength(255).WithMessage(msg.Format("MaxLength", 255));

            RuleFor(x => x.ArbName)
                .MaximumLength(255).WithMessage(msg.Format("MaxLength", 255));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(255).WithMessage(msg.Format("MaxLength", 255));

            RuleFor(x => x.PhoneKey)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.ISOAlpha2)
                .MaximumLength(2).WithMessage(msg.Format("MaxLength", 2));

            RuleFor(x => x.ISOAlpha3)
                .MaximumLength(3).WithMessage(msg.Format("MaxLength", 3));

            RuleFor(x => x.Languages)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.Continent)
                .MaximumLength(10).WithMessage(msg.Format("MaxLength", 10));

            RuleFor(x => x.CurrencyId)
                .GreaterThan(0).When(x => x.CurrencyId.HasValue)
                .WithMessage(msg.Get("CurrencyRequired"));

            RuleFor(x => x.NationalityId)
                .GreaterThan(0).When(x => x.NationalityId.HasValue)
                .WithMessage(msg.Get("NationalityRequired"));

            RuleFor(x => x.RegionId)
                .GreaterThan(0).When(x => x.RegionId.HasValue)
                .WithMessage(msg.Get("RegionRequired"));

            RuleFor(x => x.CapitalId)
                .GreaterThan(0).When(x => x.CapitalId.HasValue)
                .WithMessage(msg.Get("CapitalRequired"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(msg.Format("MaxLength", 2048));
        }
    }
}