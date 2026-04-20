using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using FluentValidation;

namespace Application.System.MasterData.Branch.Validators
{
    public class CreateBranchValidator : AbstractValidator<CreateBranchDto>
    {
        private readonly IBranchRepository _repo;

        public CreateBranchValidator(IValidationMessages msg, IBranchRepository repo)
        {
            _repo = repo;
            RuleFor(x => x.Code)
   .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
   .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
   .MustAsync(async (code, cancellation) =>
   {
       return !await _repo.CodeExistsAsync(code);
   })
   .WithMessage(x => msg.Format("CodeExists", msg.Get("Branch"), x.Code));

            RuleFor(x => x.EngName)
       .NotEmpty().WithMessage(x => msg.Get("EngNameRequired"))
       .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
       .MustAsync(async (engName, cancellation) =>
       {
           return await _repo.IsEngNameUniqueAsync(engName,null, cancellation);
       })
       .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName));

            RuleFor(x => x.ArbName)
               .NotEmpty().WithMessage(x => msg.Get("ArbNameRequired"))
               .MaximumLength(100).WithMessage(x => msg.Format("MaxLength", 100))
               .MustAsync(async (arbname, cancellation) =>
               {
                   return await _repo.IsArbNameUniqueAsync(arbname,null, cancellation);
               })
               .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName));
            RuleFor(x => x.ParentId)
                .GreaterThan(0).When(x => x.ParentId.HasValue)
                .WithMessage(x => msg.Get("ParentBranchRequired"));
            RuleFor(x => x.CountryId)
                .GreaterThan(0).When(x => x.CountryId.HasValue)
                .WithMessage(x => msg.Get("CountryRequired"));
            RuleFor(x => x.CityId)
                .GreaterThan(0).When(x => x.CityId.HasValue)
                .WithMessage(x => msg.Get("CityRequired"));
            RuleFor(x => x.PrepareDay)
                .InclusiveBetween(1, 31).When(x => x.PrepareDay.HasValue)
                .WithMessage(x => msg.Get("PrepareDayRange"));
            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(x => msg.Format("MaxLength", 2048));
             
        }
    }
}