using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Branch.Queries
{
    public static class GetBranchById
    {
        public record Query(int Id) : IRequest<BranchDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("Branch ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, BranchDto>
        {
            private readonly IBranchRepository _repo;

            public Handler(IBranchRepository repo)
            {
                _repo = repo;
            }

            public async Task<BranchDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var branch = await _repo.GetByIdAsync(request.Id);
                if (branch == null)
                    throw new Exception($"Branch with ID {request.Id} not found");

                return new BranchDto(
                    Id: branch.Id,
                    Code: branch.Code,
                    CompanyId: branch.CompanyId,
                    CompanyName: branch.Company?.EngName ?? branch.Company?.ArbName,
                    EngName: branch.EngName,
                    ArbName: branch.ArbName,
                    ArbName4S: branch.ArbName4S,
                    ParentId: branch.ParentId,
                    ParentBranchName: branch.ParentBranch?.EngName ?? branch.ParentBranch?.ArbName,
                    CountryId: branch.CountryId,
                    CityId: branch.CityId,
                    DefaultAbsent: branch.DefaultAbsent,
                    PrepareDay: branch.PrepareDay,
                    AffectPeriod: branch.AffectPeriod,
                    Remarks: branch.Remarks,
                    RegDate: branch.RegDate,
                    CancelDate: branch.CancelDate,
                    IsActive: branch.IsActive()
                );
            }
        }
    }
}