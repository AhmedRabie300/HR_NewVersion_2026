using Application.Common.Abstractions;
using Application.System.MasterData.Nationality.Dtos;
using FluentValidation;
using Application.Abstractions;
using Application.System.MasterData.Abstractions;

namespace Application.System.MasterData.Nationality.Validators
{
    public class CreateNationalityValidator : AbstractValidator<CreateNationalityDto>
    {
        private readonly INationalityRepository _repo;
        public CreateNationalityValidator(IValidationMessages msg,INationalityRepository repo)
        {
            _repo = repo;
            RuleFor(x => x.Code)
.NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
.MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
.MustAsync(async (code, cancellation) =>
{
return !await _repo.CodeExistsAsync(code);
})
.WithMessage(x => msg.Format("CodeExists", msg.Get("Nationality"), x.Code));


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

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(msg.Format("MaxLength", 2048));

            RuleFor(x => x.TravelRoute)
                .GreaterThan(0).When(x => x.TravelRoute.HasValue)
                .WithMessage(msg.Get("TravelRouteMustBePositive"));

            RuleFor(x => x.TravelClass)
                .GreaterThan(0).When(x => x.TravelClass.HasValue)
                .WithMessage(msg.Get("TravelClassMustBePositive"));

            RuleFor(x => x.TicketAmount)
                .GreaterThan(0).When(x => x.TicketAmount.HasValue)
                .WithMessage(msg.Get("TicketAmountMustBePositive"));
        }
    }
}