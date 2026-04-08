// Application/System/MasterData/Currency/Queries/ListCurrencies.cs
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Currency.Queries
{
    public static class ListCurrencies
    {
        public record Query : IRequest<List<CurrencyDto>>;

        public class Handler : IRequestHandler<Query, List<CurrencyDto>>
        {
            private readonly ICurrencyRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(ICurrencyRepository repo, IHttpContextAccessor httpContextAccessor)
            {
                _repo = repo;
                _httpContextAccessor = httpContextAccessor;
            }

            private int GetRequiredCompanyId()
            {
                var context = _httpContextAccessor.HttpContext;
                var companyId = context?.Items["CompanyId"] as int?;
                if (!companyId.HasValue)
                    throw new UnauthorizedAccessException("Company ID is required in request header (X-CompanyId)");
                return companyId.Value;
            }

            public async Task<List<CurrencyDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();

                var items = await _repo.GetAllAsync(companyId);

                return items.Select(x => new CurrencyDto(
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
            }
        }
    }
}