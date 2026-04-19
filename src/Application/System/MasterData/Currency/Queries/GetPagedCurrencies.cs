// Application/System/MasterData/Currency/Queries/GetPagedCurrencies.cs
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Currency.Queries
{
    public static class GetPagedCurrencies
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<CurrencyDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<CurrencyDto>>
        {
            private readonly ICurrencyRepository _repo;
            private readonly IContextService _ContextService;

            public Handler(ICurrencyRepository repo, IContextService ContextService)
            {
                _repo = repo;
                _ContextService = ContextService;
            }

            public async Task<PagedResult<CurrencyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    companyId
                );

                var items = pagedResult.Items.Select(x => new CurrencyDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName ?? x.Company?.ArbName,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    EngSymbol: x.EngSymbol,
                    ArbSymbol: x.ArbSymbol,
                    DecimalFraction: x.DecimalFraction,
                    DecimalEngName: x.DecimalEngName,
                    DecimalArbName: x.DecimalArbName,
                    Amount: x.Amount,
                    NoDecimalPlaces: x.NoDecimalPlaces,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
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