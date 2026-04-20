using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.TransactionsGroup.Dtos;
using FluentValidation;

namespace Application.System.HRS.TransactionsGroup.Validators
{
    public class CreateTransactionsGroupValidator : AbstractValidator<CreateTransactionsGroupDto>
    {
        private readonly ITransactionsGroupRepository _repo;
        public CreateTransactionsGroupValidator(IValidationMessages msg, ITransactionsGroupRepository repo)
        {
            _repo = repo;
            RuleFor(x => x.Code)
          .NotEmpty().WithMessage(x => msg.Get("CodeRequired"))
          .MaximumLength(50).WithMessage(x => msg.Format("MaxLength", 50))
          .MustAsync(async (code, cancellation) =>
          {
              return !await _repo.CodeExistsAsync(code);
          })
          .WithMessage(x => msg.Format("CodeExists", msg.Get("TransactionsGroup"), x.Code));

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
                  .MaximumLength(100).WithMessage(string.Format(msg.Get("MaxLength"), 100));

            RuleFor(x => x.Remarks)
                .MaximumLength(2048).WithMessage(string.Format(msg.Get("MaxLength"), 2048));
        }
    }
}