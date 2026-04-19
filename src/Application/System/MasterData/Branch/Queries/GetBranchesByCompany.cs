using Application.Common;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Branch.Queries
{
    public static class GetBranchesByCompany
    {
        public record Query() : IRequest<List<BranchDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
        
            }
        }

        public class Handler : IRequestHandler<Query, List<BranchDto>>
        {
            private readonly IBranchRepository _repo;
            private readonly ICompanyRepository _companyRepo;

            public Handler(IBranchRepository repo, ICompanyRepository companyRepo)
            {
                _repo = repo;
                _companyRepo = companyRepo;
            }

            public async Task<List<BranchDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                 
                var branches = await _repo.GetAllAsync();

                return branches.Select(b => new BranchDto(
                    Id: b.Id,
                    Code: b.Code,
                    CompanyId: b.CompanyId,
                    CompanyName:b.Company.EngName,
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