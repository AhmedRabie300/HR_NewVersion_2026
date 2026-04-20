using Application.System.MasterData.Profession.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using Application.Abstractions;
using Application.System.MasterData.Abstractions;

namespace Application.System.MasterData.Profession.Validators
{
    public class UpdateProfessionValidator : AbstractValidator<UpdateProfessionDto>
    {
        private readonly IProfessionRepository _repo;
        public UpdateProfessionValidator(IValidationMessages msg,IProfessionRepository repo)
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

            RuleFor(x => x)
                .Must(HaveAtLeastOneField)
                .WithMessage(x => msg.Get("AtLeastOneField"));
        }

        private bool HaveAtLeastOneField(UpdateProfessionDto dto)
        {
            return dto.EngName != null ||
                   dto.ArbName != null ||
                   dto.ArbName4S != null ||
                   dto.Remarks != null;
        }
    }
}