using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using MediatR;

namespace Application.System.MasterData.Currency.Queries
{
    public static class GetPagedCurrencies
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm, int? CompanyId)
            : IRequest<PagedResult<CurrencyDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<CurrencyDto>>
        {
            private readonly ICurrencyRepository _repo;

            public Handler(ICurrencyRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<CurrencyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.CompanyId
                );

                var items = pagedResult.Items.Select(x => new CurrencyDto(
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

                return new PagedResult<CurrencyDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}