using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using Application.System.MasterData.Currency.Validators;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Currency.Commands
{
    public static class UpdateCurrency
    {
        public record Command(UpdateCurrencyDto Data, int Lang = 1) : IRequest<Unit>;



        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICurrencyRepository _repo;
            private readonly ILocalizationService _localizer;

            public Handler(ICurrencyRepository repo, ILocalizationService localizer)
            {
                _repo = repo;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", request.Lang),
                        _localizer.GetMessage("Currency", request.Lang),
                        request.Data.Id));

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.EngSymbol,
                    request.Data.ArbSymbol,
                    request.Data.DecimalFraction,
                    request.Data.DecimalEngName,
                    request.Data.DecimalArbName,
                    request.Data.Amount,
                    request.Data.NoDecimalPlaces,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}