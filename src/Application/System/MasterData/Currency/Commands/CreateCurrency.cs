using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using Application.System.MasterData.Currency.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Currency.Commands
{
    public static class CreateCurrency
    {
        public record Command(CreateCurrencyDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data).SetValidator(new CreateCurrencyValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ICurrencyRepository _repo;
             private readonly IValidationMessages _msg;
            private readonly IContextService _contextService;
            public Handler(
                ICurrencyRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;

            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {

                var codeExists = await _repo.CodeExistsAsync(request.Data.Code);
                if (codeExists)
                {
                    throw new ConflictException(_msg.CodeExists("Currency", request.Data.Code));
                }

var entity = new Domain.System.MasterData.Currency(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    engSymbol: request.Data.EngSymbol,
                    arbSymbol: request.Data.ArbSymbol,
                    decimalFraction: request.Data.DecimalFraction,
                    decimalEngName: request.Data.DecimalEngName,
                    decimalArbName: request.Data.DecimalArbName,
                    amount: request.Data.Amount,
                    noDecimalPlaces: request.Data.NoDecimalPlaces,
                    remarks: request.Data.Remarks
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}