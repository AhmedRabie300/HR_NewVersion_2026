using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Department.Queries
{
    public static class GetDepartmentsByCompany
    {
         public record Query : IRequest<List<DepartmentDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
 
        }

        public class Handler : IRequestHandler<Query, List<DepartmentDto>>
        {
            private readonly IDepartmentRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(
                IDepartmentRepository repo,
                ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }
               

            

            public async Task<List<DepartmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
               

                var departments = await _repo.GetByCompanyIdAsync();

                return departments.Select(d => new DepartmentDto(
                    Id: d.Id,
                    Code: d.Code,
                    CompanyId: d.CompanyId,
                    EngName: d.EngName,
                    ArbName: d.ArbName,
                    ArbName4S: d.ArbName4S,
                    ParentId: d.ParentId,
                    ParentDepartmentName: d.ParentDepartment != null ? (_currentUser.Language == 2 ? d.ParentDepartment.ArbName : d.ParentDepartment.EngName) : null,
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