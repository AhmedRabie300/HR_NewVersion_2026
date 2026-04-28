using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Queries
{
    public static class GetEmployeeClassVacations
    {
        public record Query(int EmployeeClassId) : IRequest<List<EmployeeClassVacationDto>>;

        public class Handler : IRequestHandler<Query, List<EmployeeClassVacationDto>>
        {
            private readonly IEmployeeClassRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IEmployeeClassRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<EmployeeClassVacationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var vacations = await _repo.GetVacationsByClassIdAsync(request.EmployeeClassId);

                return vacations.Select(v => new EmployeeClassVacationDto(
                    Id: v.Id,
                    EmployeeClassId: v.EmployeeClassId,
                    VacationTypeId: v.VacationTypeId,
                    VacationTypeName: lang == 2 ? v.VacationType?.ArbName : v.VacationType?.EngName,
                    DurationDays: v.DurationDays,
                    RequiredWorkingMonths: v.RequiredWorkingMonths,
                    FromMonth: v.FromMonth,
                    ToMonth: v.ToMonth,
                    Remarks: v.Remarks,
                    TicketsRnd: v.TicketsRnd,
                    DependantTicketRnd: v.DependantTicketRnd,
                    MaxKeepDays: v.MaxKeepDays,
                    RegDate: v.RegDate,
                    CancelDate: v.CancelDate,
                    IsActive: v.IsActive()
                )).ToList();
            }
        }
    }
}