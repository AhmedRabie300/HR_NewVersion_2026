using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using FluentValidation;

namespace Application.System.MasterData.BloodGroup.Validators
{
    public class CreateBloodGroupValidator : AbstractValidator<CreateBloodGroupDto>
    {
        private readonly IBloodGroupRepository _repo;

        public CreateBloodGroupValidator(IValidationMessages msg, IBloodGroupRepository repo)
        {
            _repo = repo;
            RuleFor(x => x.Code)
           .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
           .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
           .MustAsync(async (code, cancellation) =>
           {
               return !await _repo.CodeExistsAsync(code);
           })
           .WithMessage(x => msg.Format("CodeExists", msg.Get("BloodGroup"), x.Code));

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
        }
    }
}