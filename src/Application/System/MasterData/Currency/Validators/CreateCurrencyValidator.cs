using Application.Common.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using FluentValidation;
using Application.Abstractions;
using Application.System.MasterData.Abstractions;

namespace Application.System.MasterData.Currency.Validators
{
    public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyDto>
    {
        private readonly ICurrencyRepository _repo;
        public CreateCurrencyValidator(IValidationMessages msg,ICurrencyRepository repo)
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

            RuleFor(x => x.EngSymbol)
                .MaximumLength(10).WithMessage(msg.Format("MaxLength", 10));

            RuleFor(x => x.ArbSymbol)
                .MaximumLength(10).WithMessage(msg.Format("MaxLength", 10));

            RuleFor(x => x.DecimalFraction)
                .GreaterThanOrEqualTo(0).WithMessage(msg.Get("DecimalFractionRequired"));

            RuleFor(x => x.DecimalEngName)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.DecimalArbName)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.NoDecimalPlaces)
                .InclusiveBetween(0, 4).When(x => x.NoDecimalPlaces.HasValue)
                .WithMessage(msg.Get("DecimalPlacesRange"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(msg.Format("MaxLength", 2048));
        }
    }
}