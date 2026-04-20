using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Bank.Dtos;
using Application.System.MasterData.Bank.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Bank.Commands
{
    public static class UpdateBank
    {
        public record Command(UpdateBankDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IBankRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateBankValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IBankRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IBankRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity is null)
                    throw new NotFoundException(_msg.NotFound("Bank", request.Data.Id));

                entity.Update(
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