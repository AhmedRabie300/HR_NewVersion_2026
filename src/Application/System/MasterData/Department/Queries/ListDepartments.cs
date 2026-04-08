// Application/System/MasterData/Department/Queries/ListDepartments.cs
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Department.Queries
{
    public static class ListDepartments
    {
        public record Query : IRequest<List<DepartmentDto>>;

        public class Handler : IRequestHandler<Query, List<DepartmentDto>>
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
                    throw new UnauthorizedAccessException("Company ID is required in request header (X-CompanyId)");
                return companyId.Value;
            }

            public async Task<List<DepartmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();

                var items = await _repo.GetAllAsync(companyId);

                return items.Select(x => new DepartmentDto(
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
            }
        }
    }
}