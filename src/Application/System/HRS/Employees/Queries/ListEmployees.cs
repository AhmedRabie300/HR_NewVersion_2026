using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Employees.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.Employees.Queries
{
    public static class ListEmployees
    {
        public record Query : IRequest<List<EmployeeListDto>>;

        public class Handler : IRequestHandler<Query, List<EmployeeListDto>>
        {
            private readonly IEmployeeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IEmployeeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<EmployeeListDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var items = await _repo.GetByCompanyIdAsync();

                return items.Select(x => new EmployeeListDto(
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
            }
        }
    }
}