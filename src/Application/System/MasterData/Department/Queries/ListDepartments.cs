// Application/System/MasterData/Department/Queries/ListDepartments.cs
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using MediatR;

namespace Application.System.MasterData.Department.Queries
{
    public static class ListDepartments
    {
        public record Query : IRequest<List<DepartmentDto>>;

        public class Handler : IRequestHandler<Query, List<DepartmentDto>>
        {
            private readonly IDepartmentRepository _repo;

            public Handler(IDepartmentRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<DepartmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var departments = await _repo.GetAllAsync();

                return departments.Select(d => new DepartmentDto(
                    Id: d.Id,
                    Code: d.Code,
                    CompanyId: d.CompanyId,
                    CompanyName: d.Company?.EngName ?? d.Company?.ArbName,
                    EngName: d.EngName,
                    ArbName: d.ArbName,
                    ArbName4S: d.ArbName4S,
                    ParentId: d.ParentId,
                    ParentDepartmentName: d.ParentDepartment?.EngName ?? d.ParentDepartment?.ArbName,
                    Remarks: d.Remarks,
                    CostCenterCode: d.CostCenterCode,
                    RegDate: d.RegDate,
                    CancelDate: d.CancelDate,
                    IsActive: d.IsActive()
                )).ToList();
            }
        }
    }
}