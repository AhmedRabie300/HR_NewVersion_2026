using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using MediatR;

namespace Application.System.MasterData.Currency.Queries
{
    public static class ListCurrencies
    {
        public record Query : IRequest<List<CurrencyDto>>;

        public class Handler : IRequestHandler<Query, List<CurrencyDto>>
        {
            private readonly ICurrencyRepository _repo;

            public Handler(ICurrencyRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<CurrencyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAllAsync();

                return items.Select(x => new CurrencyDto(
                    x.Id,
                    x.Code,
                    x.EngName,
                    x.ArbName,
                    x.ArbName4S,
                    x.EngSymbol,
                    x.ArbSymbol,
                    x.DecimalFraction,
                    x.DecimalEngName,
                    x.DecimalArbName,
                    x.Amount,
                    x.NoDecimalPlaces,
                    x.CompanyId,
                    x.Company?.EngName ?? x.Company?.ArbName,
                    x.Remarks,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();
            }
        }
    }
}