using Application.System.MasterData.Abstractions;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.ContractType.Commands
{
    public static class SoftDeleteContractType
    {
        public record Command(int Id) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContractTypeRepository _repo;

            public Validator(IValidationMessages msg, IContractTypeRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"))
                    .MustAsync(async (id, cancellation) => !await _repo.IsUsedInContractsAsync(id))
                    .WithMessage(msg.Get("ContractTypeUsedInContracts"));

            }
        }


        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IContractTypeRepository _repo;

            public Handler(IContractTypeRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _repo.SoftDeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}