using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Commands
{
    public static class UpdateEndOfService
    {
        public record Command(UpdateEndOfServiceDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEndOfServiceRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateEndOfServiceValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IEndOfServiceRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IEndOfServiceRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("EndOfService", request.Data.Id));

                entity.Update(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    remarks: request.Data.Remarks,
                    extraTransactionId: request.Data.ExtraTransactionId,
                    excludedFromSSRequests: request.Data.ExcludedFromSSRequests
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}