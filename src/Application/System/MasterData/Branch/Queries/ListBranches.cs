using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using MediatR;

namespace Application.System.MasterData.Branch.Queries
{
    public static class ListBranches
    {
        public record Query : IRequest<List<BranchDto>>;

        public class Handler : IRequestHandler<Query, List<BranchDto>>
        {
            private readonly IBranchRepository _repo;
            private readonly IContextService _contextService;

            public Handler(IBranchRepository repo, IContextService contextService)
            {
                _repo = repo;
                _contextService = contextService;
            }

            public async Task<List<BranchDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _contextService.GetCurrentCompanyId();

               
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