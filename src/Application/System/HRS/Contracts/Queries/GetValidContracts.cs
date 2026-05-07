using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using Domain.System.HRS.Employees;
using MediatR;

namespace Application.System.HRS.Contracts.Queries
{
    public static class GetValidContracts
    {
        public record Query(int? EmployeeId = null) : IRequest<List<ContractListDto>>;

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
                var companyId = _currentUser.CompanyId;
                var now = DateTime.Now;

                // Get all active contracts for the company
                var allContracts = await _repo.GetByCompanyIdAsync();

                // Filter valid contracts
                var validContracts = allContracts
                    .Where(x => x.IsActive() && (!x.EndDate.HasValue || x.EndDate.Value >= now));

                // Filter by employee if specified
                if (request.EmployeeId.HasValue && request.EmployeeId.Value > 0)
                {
                    validContracts = validContracts.Where(x => x.EmployeeId == request.EmployeeId.Value);
                }

                return validContracts.Select(x => new ContractListDto(
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