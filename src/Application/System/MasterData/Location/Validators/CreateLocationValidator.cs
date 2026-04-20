using Application.Common.Abstractions;
using Application.System.MasterData.Location.Dtos;
using FluentValidation;
using Application.Abstractions;
using Application.System.MasterData.Abstractions;

namespace Application.System.MasterData.Location.Validators
{
    public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
    {
        private readonly ILocationRepository _repo;
        public CreateLocationValidator(IValidationMessages msg,ILocationRepository repo)
        {
            _repo = repo;

            RuleFor(x => x.Code)
.NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
.MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
.MustAsync(async (code, cancellation) =>
{
return !await _repo.CodeExistsAsync(code);
})
.WithMessage(x => msg.Format("CodeExists", msg.Get("Location"), x.Code));


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

            RuleFor(x => x.CityId)
                .GreaterThan(0).When(x => x.CityId.HasValue)
                .WithMessage(msg.Get("CityRequired"));

            RuleFor(x => x.BranchId)
                .GreaterThan(0).When(x => x.BranchId.HasValue)
                .WithMessage(msg.Get("BranchRequired"));

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).When(x => x.DepartmentId.HasValue)
                .WithMessage(msg.Get("DepartmentRequired"));

            RuleFor(x => x.CostCenterCode1)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode2)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode3)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.CostCenterCode4)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(msg.Format("MaxLength", 2048));
        }
    }
}