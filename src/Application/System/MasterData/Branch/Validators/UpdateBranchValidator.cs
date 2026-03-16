using Application.System.MasterData.Branch.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Branch.Validators
{
    public class UpdateBranchValidator : AbstractValidator<UpdateBranchDto>
    {
        public UpdateBranchValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Valid branch ID is required");

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

            RuleFor(x => x.ParentId)
                .GreaterThan(0).When(x => x.ParentId.HasValue)
                .WithMessage("Parent ID must be greater than 0");

            RuleFor(x => x.CountryId)
                .GreaterThan(0).When(x => x.CountryId.HasValue)
                .WithMessage("Country ID must be greater than 0");

            RuleFor(x => x.CityId)
                .GreaterThan(0).When(x => x.CityId.HasValue)
                .WithMessage("City ID must be greater than 0");

            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage("Prepare day must be between 1 and 31");

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage("At least one field must be provided for update");
        }

        private bool HaveAtLeastOneField(UpdateBranchDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.ParentId.HasValue ||
                   dto.CountryId.HasValue ||
                   dto.CityId.HasValue ||
                   dto.DefaultAbsent.HasValue ||
                   dto.PrepareDay.HasValue ||
                   dto.AffectPeriod.HasValue ||
                   dto.Remarks != null;
        }
    }
}