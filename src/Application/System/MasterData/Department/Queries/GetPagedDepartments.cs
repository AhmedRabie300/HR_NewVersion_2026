using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using MediatR;

namespace Application.System.MasterData.Department.Queries
{
    public static class GetPagedDepartments
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm, int? CompanyId) : IRequest<PagedResult<DepartmentDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<DepartmentDto>>
        {
            private readonly IDepartmentRepository _repo;

            public Handler(IDepartmentRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<DepartmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.CompanyId
                );

                var items = pagedResult.Items.Select(d => new DepartmentDto(
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

                return new PagedResult<DepartmentDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}