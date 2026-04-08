using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Branch.Queries
{
    public static class ListBranches
    {
        public record Query : IRequest<List<BranchDto>>;

        public class Handler : IRequestHandler<Query, List<BranchDto>>
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

            public async Task<List<BranchDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();

                var branches = await _repo.GetAllAsync(companyId);

                return branches.Select(b => new BranchDto(
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
            }
        }
    }
}