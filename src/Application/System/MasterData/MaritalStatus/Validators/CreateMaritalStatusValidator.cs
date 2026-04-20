using Application.Common.Abstractions;
using Application.System.MasterData.MaritalStatus.Dtos;
using FluentValidation;
using Application.Abstractions;
using Application.System.MasterData.Abstractions;

namespace Application.System.MasterData.MaritalStatus.Validators
{
    public class CreateMaritalStatusValidator : AbstractValidator<CreateMaritalStatusDto>
    {
        private readonly IMaritalStatusRepository _repo;
        public CreateMaritalStatusValidator(IValidationMessages msg,IMaritalStatusRepository repo)
        {
            _repo = repo;
            RuleFor(x => x.Code)
                .MaximumLength(50).WithMessage(msg.Format("MaxLength", 50));

            RuleFor(x => x.EngName)
        .MustAsync(async (engName, cancellation) =>
        {
            if (string.IsNullOrWhiteSpace(engName)) return true;
            return await _repo.IsEngNameUniqueAsync(engName, cancellation);
        })
       .When(x => !string.IsNullOrWhiteSpace(x.EngName))
       .WithMessage(x => msg.Format("EngNameAlreadyExists", x.EngName))
       .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName)
                   .MustAsync(async (arbname, cancellation) =>
                   {
                       if (string.IsNullOrWhiteSpace(arbname)) return true;
                       return await _repo.IsArbNameUniqueAsync(arbname, cancellation);
                   })
                .When(x => !string.IsNullOrWhiteSpace(x.ArbName))
                .WithMessage(x => msg.Format("ArbNameAlreadyExists", x.ArbName))
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.ArbName4S)
                .MaximumLength(100).WithMessage(msg.Format("MaxLength", 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(msg.Format("MaxLength", 2048));
        }
    }
}