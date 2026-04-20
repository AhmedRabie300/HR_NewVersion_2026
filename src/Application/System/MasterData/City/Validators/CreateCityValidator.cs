using Application.Common.Abstractions;
using Application.System.MasterData.City.Dtos;
using FluentValidation;
using Application.Abstractions;
using Application.System.MasterData.Abstractions;

namespace Application.System.MasterData.City.Validators
{
    public class CreateCityValidator : AbstractValidator<CreateCityDto>
    {
        private readonly ICityRepository _repo;

        public CreateCityValidator(IValidationMessages msg,ICityRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Code)
.NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
.MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
.MustAsync(async (code, cancellation) =>
{
return !await _repo.CodeExistsAsync(code);
})
.WithMessage(x => msg.Format("CodeExists", msg.Get("City"), x.Code));


            RuleFor(x => x.EngName)
       .NotEmpty().WithMessage(x => msg.Get("EngNameRequired"))
       .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
       .MustAsync(async (engName, cancellation) =>
       {
           return await _repo.IsEngNameUniqueAsync(engName, null, cancellation);
       })
       .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
               .NotEmpty().WithMessage(x => msg.Get("ArbNameRequired"))
               .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
               .MustAsync(async (arbname, cancellation) =>
               {
                   return await _repo.IsArbNameUniqueAsync(arbname, null, cancellation);
               })
               .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.PhoneKey)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.TimeZone)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.CountryId)
                .GreaterThan(0).When(x => x.CountryId.HasValue)
                .WithMessage(msg.Get("CountryRequired"));

            RuleFor(x => x.RegionId)
                .GreaterThan(0).When(x => x.RegionId.HasValue)
                .WithMessage(msg.Get("RegionRequired"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(msg.Format("MaxLength", 2048));
        }
    }
}