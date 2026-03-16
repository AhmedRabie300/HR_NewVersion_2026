using Application.Common;
using Application.UARbac.FormControls.Validators;
using Application.UARbac.Abstractions;
using Application.UARbac.FormControls.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.FormControls;

public static class Update
{
    public sealed record Command(UpdateFormControlDto Dto) : IRequest<Unit>;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Dto)
                  .NotNull().WithMessage("FormControl data is required.")
                  .SetValidator(new UpdateFormControlValidator());
        }
    }

    public sealed class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IFormControlRepository _repo;
        public Handler(IFormControlRepository repo) => _repo = repo;

        public async Task<Unit> Handle(Command request, CancellationToken ct)
        {
            var dto = request.Dto;
 ;
            var entity = await _repo.GetByIdAsync(dto.Id, ct);
            if (entity == null)
            {
                 throw new NotFoundException("FormControl", dto.Id);
            }
            // Only updates these fields, nothing else
            entity.UpdateUiSettings(dto.EngCaption, dto.ArbCaption, dto.IsDisabled, dto.IsHide);

            await _repo.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}
