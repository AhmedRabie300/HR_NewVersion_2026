using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Department.Queries
{
    public static class GetDepartmentById
    {
        public record Query(int Id) : IRequest<DepartmentDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("Department ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, DepartmentDto>
        {
            private readonly IDepartmentRepository _repo;

            public Handler(IDepartmentRepository repo)
            {
                _repo = repo;
            }

            public async Task<DepartmentDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var department = await _repo.GetByIdAsync(request.Id);
                if (department == null)
                    throw new Exception($"Department with ID {request.Id} not found");

                return new DepartmentDto(
                    Id: department.Id,
                    Code: department.Code,
                    CompanyId: department.CompanyId,
                    CompanyName: department.Company?.EngName ?? department.Company?.ArbName,
                    EngName: department.EngName,
                    ArbName: department.ArbName,
                    ArbName4S: department.ArbName4S,
                    ParentId: department.ParentId,
                    ParentDepartmentName: department.ParentDepartment?.EngName ?? department.ParentDepartment?.ArbName,
                    Remarks: department.Remarks,
                    CostCenterCode: department.CostCenterCode,
                    RegDate: department.RegDate,
                    CancelDate: department.CancelDate,
                    IsActive: department.IsActive()
                );
            }
        }
    }
}