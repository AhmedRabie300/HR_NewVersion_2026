using Application.Common.Abstractions;
using Application.System.MasterData.Country.Dtos;
using FluentValidation;
using Application.Abstractions;
using Application.System.MasterData.Abstractions;

namespace Application.System.MasterData.Country.Validators
{
    public class CreateCountryValidator : AbstractValidator<CreateCountryDto>
    {
        private readonly ICountryRepository _repo;
        public CreateCountryValidator(IValidationMessages msg,ICountryRepository repo)
        {
            _repo = repo;
            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));


            RuleFor(x => x.EngName)
       .NotEmpty().WithMessage(x => msg.Get("EngNameRequired"))
       .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
       .MustAsync(async (engName, cancellation) =>
       {
           return await _repo.IsEngNameUniqueAsync(engName, cancellation);
       })
       .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
               .NotEmpty().WithMessage(x => msg.Get("ArbNameRequired"))
               .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
               .MustAsync(async (arbname, cancellation) =>
               {
                   return await _repo.IsArbNameUniqueAsync(arbname, cancellation);
               })
               .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

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

            //RuleFor(x => x.RegionId)
            //    .GreaterThan(0).When(x => x.RegionId.HasValue)
            //    .WithMessage(msg.Get("RegionRequired"));

            RuleFor(x => x.CapitalId)
                .GreaterThan(0).When(x => x.CapitalId.HasValue)
                .WithMessage(msg.Get("CapitalRequired"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(msg.Format("MaxLength", 2048));
        }
    }
}