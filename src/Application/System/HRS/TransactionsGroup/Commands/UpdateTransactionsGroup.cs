using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.TransactionsGroup.Dtos;
using Application.System.HRS.TransactionsGroup.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.TransactionsGroup.Commands
{
    public static class UpdateTransactionsGroup
    {
        public record Command(UpdateTransactionsGroupDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, ITransactionsGroupRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateTransactionsGroupValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ITransactionsGroupRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                ITransactionsGroupRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("TransactionsGroup", request.Data.Id));

                entity.Update(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}