using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Bank.Dtos;
using Application.System.MasterData.Bank.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Bank.Commands
{
    public static class CreateBank
    {
        public record Command(
            CreateBankDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,IBankRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateBankValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IBankRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                IBankRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {                              

                var entity = new Domain.System.MasterData.Bank(
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