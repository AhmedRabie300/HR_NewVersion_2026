using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Contracts.Dtos;
using MediatR;

namespace Application.System.HRS.Contracts.Queries
{
    public static class GetPagedContracts
    {
        public record Query(
            int PageNumber = 1,
            int PageSize = 20,
            string? SearchTerm = null
        ) : IRequest<PagedResult<ContractListDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<ContractListDto>>
        {
            private readonly IContractRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IContractRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<PagedResult<ContractListDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new ContractListDto(
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

                return new PagedResult<ContractListDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}