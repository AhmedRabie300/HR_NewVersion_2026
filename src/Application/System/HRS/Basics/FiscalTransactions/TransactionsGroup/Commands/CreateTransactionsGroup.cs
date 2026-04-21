using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.TransactionsGroup.Dtos;
using Application.System.HRS.Basics.FiscalTransactions.TransactionsGroup.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.TransactionsGroup.Commands
{
    public static class CreateTransactionsGroup
    {
        public record Command(
            CreateTransactionsGroupDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, ITransactionsGroupRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateTransactionsGroupValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
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

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
              
                var entity = new Domain.System.HRS.Basics.FiscalTransactions.TransactionsGroup(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    remarks: request.Data.Remarks
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}