using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.EmployeesClasses.Dtos;
using Application.System.HRS.Basics.GradeAndClasses.EmployeesClasses.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.EmployeesClasses.Queries
{
    public static class GetEmployeeClassDelays
    {
        public record Query(int ClassId) : IRequest<List<EmployeeClassDelayDto>>;

        public class Handler : IRequestHandler<Query, List<EmployeeClassDelayDto>>
        {
            private readonly IEmployeeClassRepository _repo;

            public Handler(IEmployeeClassRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<EmployeeClassDelayDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var delays = await _repo.GetDelaysByClassIdAsync(request.ClassId);

                return delays.Select(d => new EmployeeClassDelayDto(
                    Id: d.Id,
                    ClassId: d.ClassId,
                    FromMin: d.FromMin,
                    ToMin: d.ToMin,
                    PunishPCT: d.PunishPCT,
                    Remarks: d.Remarks,
                    RegDate: d.RegDate,
                    CancelDate: d.CancelDate,
                    IsActive: d.IsActive()
                )).ToList();
            }
        }
    }
}