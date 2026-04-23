using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.ContractsTypes.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.ContractsTypes.Queries
{
    public static class ListContractsTypes
    {
        public record Query : IRequest<List<ContractsTypeDto>>;

        public class Handler : IRequestHandler<Query, List<ContractsTypeDto>>
        {
            private readonly IContractsTypeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IContractsTypeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<ContractsTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {

                var items = await _repo.GetByCompanyIdAsync();

                return items.Select(x => new ContractsTypeDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    CompanyId: x.CompanyId,
                    IsSpecial: x.IsSpecial,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}