// Application/System/MasterData/Profession/Queries/GetPagedProfessions.cs
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Profession.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Profession.Queries
{
    public static class GetPagedProfessions
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<ProfessionDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<ProfessionDto>>
        {
            private readonly IProfessionRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILanguageService _languageService;

            public Handler(IProfessionRepository repo, IHttpContextAccessor httpContextAccessor, ILanguageService languageService)
            {
                _repo = repo;
                _httpContextAccessor = httpContextAccessor;
                _languageService = languageService;
            }

            private int GetRequiredCompanyId()
            {
                var context = _httpContextAccessor.HttpContext;
                var companyId = context?.Items["CompanyId"] as int?;
                if (!companyId.HasValue)
                    throw new UnauthorizedAccessException("Company ID is required in request header");
                return companyId.Value;
            }

            public async Task<PagedResult<ProfessionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();
                var lang = _languageService.GetCurrentLanguage();

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    companyId
                );

                var items = pagedResult.Items.Select(x => new ProfessionDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName ?? x.Company?.ArbName,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();

                return new PagedResult<ProfessionDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}