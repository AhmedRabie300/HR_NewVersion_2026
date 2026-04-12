using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Branch.Queries
{
    public static class GetPagedBranches
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<BranchDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<BranchDto>>
        {
            private readonly IBranchRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;
            

            public Handler(IBranchRepository repo, IHttpContextAccessor httpContextAccessor)
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

            public async Task<PagedResult<BranchDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    companyId
                );

                var items = pagedResult.Items.Select(b => new BranchDto(
                    Id: b.Id,
                    Code: b.Code,
                    CompanyId: b.CompanyId,
                    CompanyName: b.Company?.EngName ?? b.Company?.ArbName,
                    EngName: b.EngName,
                    ArbName: b.ArbName,
                    ArbName4S: b.ArbName4S,
                    ParentId: b.ParentId,
                    ParentBranchName: b.ParentBranch?.EngName ?? b.ParentBranch?.ArbName,
                    CountryId: b.CountryId,
                    CityId: b.CityId,
                    DefaultAbsent: b.DefaultAbsent,
                    PrepareDay: b.PrepareDay,
                    AffectPeriod: b.AffectPeriod,
                    Remarks: b.Remarks,
                    RegDate: b.RegDate,
                    CancelDate: b.CancelDate,
                    IsActive: b.IsActive()
                )).ToList();

                return new PagedResult<BranchDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}