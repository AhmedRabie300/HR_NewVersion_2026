using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Employees.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.Employees.Queries
{
    public static class GetPagedEmployees
    {
        public record Query(
            int PageNumber = 1,
            int PageSize = 20,
            string? SearchTerm = null
        ) : IRequest<PagedResult<EmployeeListDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<EmployeeListDto>>
        {
            private readonly IEmployeeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IEmployeeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<PagedResult<EmployeeListDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new EmployeeListDto(
                    Id: x.Id,
                    Code: x.Code,
                    FullName: x.GetFullName(lang),
                    DepartmentName: lang == 2 ? x.Department?.ArbName : x.Department?.EngName,
                    BranchName: lang == 2 ? x.Branch?.ArbName : x.Branch?.EngName,
                    NationalityName: lang == 2 ? x.Nationality?.ArbName : x.Nationality?.EngName,
                    PositionName: null,
                    JoinDate: x.JoinDate,
                    Mobile: x.Mobile,
                    Email: x.Email,
                    IsActive: x.IsActive()
                )).ToList();

                return new PagedResult<EmployeeListDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}