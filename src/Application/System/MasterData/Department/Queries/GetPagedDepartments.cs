// Application/System/MasterData/Department/Queries/GetPagedDepartments.cs
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Department.Queries
{
    public static class GetPagedDepartments
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<DepartmentDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<DepartmentDto>>
        {
            private readonly IDepartmentRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IDepartmentRepository repo, IHttpContextAccessor httpContextAccessor)
            {
                _repo = repo;
                _httpContextAccessor = httpContextAccessor;
            }

            private int GetRequiredCompanyId()
            {
                var context = _httpContextAccessor.HttpContext;
                var companyId = context?.Items["CompanyId"] as int?;
                if (!companyId.HasValue)
                    throw new UnauthorizedAccessException("Company ID is required in request header");
                return companyId.Value;
            }

            public async Task<PagedResult<DepartmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    companyId
                );

                var items = pagedResult.Items.Select(x => new DepartmentDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName ?? x.Company?.ArbName,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    ParentId: x.ParentId,
                    ParentDepartmentName: x.ParentDepartment?.EngName ?? x.ParentDepartment?.ArbName,
                    Remarks: x.Remarks,
                    CostCenterCode: x.CostCenterCode,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
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