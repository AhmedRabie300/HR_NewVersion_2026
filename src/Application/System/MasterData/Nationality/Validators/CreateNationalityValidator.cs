using Application.System.MasterData.Nationality.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Nationality.Validators
{
    public class CreateNationalityValidator : AbstractValidator<CreateNationalityDto>
    {
        public CreateNationalityValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Nationality code is required")
                .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

            RuleFor(x => x.EngName)
                .MaximumLength(100).WithMessage("English name must not exceed 100 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(100).WithMessage("Arabic name must not exceed 100 characters");

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage("Arabic name (4S) must not exceed 100 characters");

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage("Remarks must not exceed 2048 characters");

            RuleFor(x => x.TravelRoute)
                .GreaterThan(0).When(x => x.TravelRoute.HasValue)
                .WithMessage("Travel route must be greater than 0");

            RuleFor(x => x.TravelClass)
                .GreaterThan(0).When(x => x.TravelClass.HasValue)
                .WithMessage("Travel class must be greater than 0");

            RuleFor(x => x.TicketAmount)
                .GreaterThan(0).When(x => x.TicketAmount.HasValue)
                .WithMessage("Ticket amount must be greater than 0");
        }
    }
}