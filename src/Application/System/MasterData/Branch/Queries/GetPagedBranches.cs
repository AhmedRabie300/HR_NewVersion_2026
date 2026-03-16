using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using MediatR;

namespace Application.System.MasterData.Branch.Queries
{
    public static class GetPagedBranches
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm, int? CompanyId) : IRequest<PagedResult<BranchDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<BranchDto>>
        {
            private readonly IBranchRepository _repo;

            public Handler(IBranchRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<BranchDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.CompanyId
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