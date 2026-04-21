using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.Interval.Dtos;
using Application.System.HRS.Basics.FiscalTransactions.Interval.Validators;
using Domain.System.HRS.Basics.FiscalTransactions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.Interval.Commands
{
    public static class UpdateInterval
    {
        public record Command(UpdateIntervalDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IIntervalRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateIntervalValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IIntervalRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IIntervalRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Interval", request.Data.Id));

                entity.Update(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    number: request.Data.Number,
                    remarks: request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}