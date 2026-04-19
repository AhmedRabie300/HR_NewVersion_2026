using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.FormControls.Dtos;
using Application.UARbac.FormControls.Validators;
using FluentValidation;
using MediatR;

namespace Application.UARbac.FormControls;

public static class Update
{
    public sealed record Command(UpdateFormControlDto Dto) : IRequest<Unit>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator(IValidationMessages msg)
        {
            RuleFor(x => x.Dto)
                .NotNull().WithMessage(msg.Get("Required"))
                .SetValidator(new UpdateFormControlValidator(msg));
        }
    }

    public sealed class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IFormControlRepository _repo;
        private readonly IValidationMessages _msg;

        public Handler(IFormControlRepository repo, IValidationMessages msg)
        {
            _repo = repo;
            _msg = msg;
        }

        public async Task<Unit> Handle(Command request, CancellationToken ct)
        {
            var dto = request.Dto;
            var entity = await _repo.GetByIdAsync(dto.Id, ct);
            if (entity == null)
                throw new NotFoundException(_msg.NotFound("FormControl", dto.Id));

            entity.UpdateUiSettings(dto.EngCaption, dto.ArbCaption, dto.IsDisabled, dto.IsHide, dto.IsCompulory);

            await _repo.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
