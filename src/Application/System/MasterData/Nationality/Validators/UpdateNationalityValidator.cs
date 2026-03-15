using Application.System.MasterData.Nationality.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Nationality.Validators
{
    public class UpdateNationalityValidator : AbstractValidator<UpdateNationalityDto>
    {
        public UpdateNationalityValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Valid nationality ID is required");

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage("English name must not exceed 100 characters");

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage("Arabic name must not exceed 100 characters");

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage("Arabic name (4S) must not exceed 100 characters");

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage("Remarks must not exceed 2048 characters");

            RuleFor(x => x.TravelRoute)
                .GreaterThan(0).When(x => x.TravelRoute.HasValue)
                .WithMessage("Travel route must be greater than 0");

            RuleFor(x => x.TravelClass)
                .GreaterThan(0).When(x => x.TravelClass.HasValue)
                .WithMessage("Travel class must be greater than 0");

            RuleFor(x => x.TicketAmount)
                .GreaterThan(0).When(x => x.TicketAmount.HasValue)
                .WithMessage("Ticket amount must be greater than 0");

             RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage("At least one field must be provided for update");
        }

        private bool HaveAtLeastOneField(UpdateNationalityDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.IsMainNationality.HasValue ||
                   dto.TravelRoute.HasValue ||
                   dto.TravelClass.HasValue ||
                   dto.Remarks != null ||
                   dto.TicketAmount.HasValue;
        }
    }
}