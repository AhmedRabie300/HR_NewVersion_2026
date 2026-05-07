using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using MediatR;

namespace Application.System.HRS.Contracts.Queries
{
    public static class GetContractsByEmployeeId
    {
        public record Query(int EmployeeId) : IRequest<List<ContractListDto>>;

        public class Handler : IRequestHandler<Query, List<ContractListDto>>
        {
            private readonly IContractRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IContractRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<ContractListDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var items = await _repo.GetByEmployeeIdAsync(request.EmployeeId);

                return items.Select(x => new ContractListDto(
                    Id: x.Id,
                    Number: x.Number,
                    EmployeeID: x.EmployeeId,
                    EmployeeName: lang == 2 ? x.Employee?.ArbName : x.Employee?.EngName,
                    ContractTypeName: lang == 2 ? x.ContractType?.ArbName : x.ContractType?.EngName,
                    StartDate: x.StartDate,
                    EndDate: x.EndDate,
                    IsCurrent: x.IsCurrent,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}