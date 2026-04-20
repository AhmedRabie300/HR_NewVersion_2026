using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Location.Validators
{
    public class UpdateLocationValidator : AbstractValidator<UpdateLocationDto>
    {
        private readonly ILocationRepository _repo;

        public UpdateLocationValidator(IValidationMessages msg, ILocationRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage(x => msg.Get("IdGreaterThanZero"));

          
             RuleFor(x => x.EngName)
                .MaximumLength(100).When(x => x.EngName != null)
                .WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (dto, engName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(engName)) return true;
                    return await _repo.IsEngNameUniqueAsync(engName, dto.Id, cancellation);
                })
                .When(x => x.EngName != null)
                .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

             RuleFor(x => x.ArbName)
                .MaximumLength(100).When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("MaxLength", 100))
                .MustAsync(async (dto, arbName, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(arbName)) return true;
                    return await _repo.IsArbNameUniqueAsync(arbName, dto.Id, cancellation);
                })
                .When(x => x.ArbName != null)
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).When(x => x.ArbName4S != null)
                .WithMessage(x => msg.Format("MaxLength", 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).When(x => x.Remarks != null)
                .WithMessage(x => msg.Format("MaxLength", 2048));

            RuleFor(x => x.CostCenterCode1)
                .MaximumLength(50).When(x => x.CostCenterCode1 != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode2)
                .MaximumLength(50).When(x => x.CostCenterCode2 != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode3)
                .MaximumLength(50).When(x => x.CostCenterCode3 != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode4)
                .MaximumLength(50).When(x => x.CostCenterCode4 != null)
                .WithMessage(x => msg.Format("MaxLength", 50));

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateLocationDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.BranchId.HasValue ||
                   dto.DepartmentId.HasValue ||
                   dto.Remarks != null ||
                   dto.CostCenterCode1 != null ||
                   dto.CostCenterCode2 != null ||
                   dto.CostCenterCode3 != null ||
                   dto.CostCenterCode4 != null;
        }
    }
}