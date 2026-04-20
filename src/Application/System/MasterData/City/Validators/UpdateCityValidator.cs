using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.City.Dtos;
using FluentValidation;

namespace Application.System.MasterData.City.Validators
{
    public class UpdateCityValidator : AbstractValidator<UpdateCityDto>
    {
        private readonly ICityRepository _repo;

        public UpdateCityValidator(IValidationMessages msg, ICityRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));

  

            RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(msg.Format("MaxLength", 100))
                .MustAsync(async (dto, engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName, dto.Id, cancellation);
                })
                .When(x => x.EngName != null)
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(msg.Format("MaxLength", 100))
                .MustAsync(async (dto, arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName, dto.Id, cancellation);
                })
                .When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.PhoneKey)
                .MaximumLength(50).When(x => x.PhoneKey != null)
                .WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.TimeZone)
                .MaximumLength(50).When(x => x.TimeZone != null)
                .WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.CountryId)
                .GreaterThan(0).When(x => x.CountryId.HasValue)
                .WithMessage(msg.Get("CountryRequired"));

            RuleFor(x => x.RegionId)
                .GreaterThan(0).When(x => x.RegionId.HasValue)
                .WithMessage(msg.Get("RegionRequired"));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(msg.Format("MaxLength", 2048));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateCityDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.PhoneKey != null ||
                   dto.RegionId.HasValue ||
                   dto.TimeZone != null ||
                   dto.CountryId.HasValue ||
                   dto.Remarks != null;
        }
    }
}