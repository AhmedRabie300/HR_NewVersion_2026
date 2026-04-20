using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationsPaidType.Dtos;
using Application.System.HRS.VacationsPaidType.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationsPaidType.Commands
{
    public static class UpdateVacationsPaidType
    {
        public record Command(UpdateVacationsPaidTypeDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IVacationsPaidTypeRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateVacationsPaidTypeValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IVacationsPaidTypeRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                IVacationsPaidTypeRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("VacationsPaidType", request.Data.Id));

                entity.Update(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}