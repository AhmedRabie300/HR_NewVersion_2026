// Application/System/MasterData/Department/Queries/GetPagedDepartments.cs
using Application.Common.Abstractions;
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
            private readonly IContextService _ContextService;

            public Handler(IDepartmentRepository repo, IContextService ContextService)
            {
                _repo = repo;
                _ContextService = ContextService;
            }

            public async Task<PagedResult<DepartmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
               //  var companyId = _ContextService.GetCurrentCompanyId();

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new DepartmentDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyId: x.CompanyId,
                    //CompanyName: x.Company?.EngName ?? x.Company?.ArbName,
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