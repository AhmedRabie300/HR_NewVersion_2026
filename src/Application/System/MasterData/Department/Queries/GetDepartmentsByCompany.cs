// Application/System/MasterData/Department/Queries/GetDepartmentsByCompany.cs
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Department.Queries
{
    public static class GetDepartmentsByCompany
    {
        public record Query(int CompanyId) : IRequest<List<DepartmentDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.CompanyId)
                    .GreaterThan(0).WithMessage("Valid company ID is required");
            }
        }

        public class Handler : IRequestHandler<Query, List<DepartmentDto>>
        {
            private readonly IDepartmentRepository _repo;
            private readonly ICompanyRepository _companyRepo;

            public Handler(IDepartmentRepository repo, ICompanyRepository companyRepo)
            {
                _repo = repo;
                _companyRepo = companyRepo;
            }

            public async Task<List<DepartmentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var company = await _companyRepo.GetByIdAsync(request.CompanyId);
                if (company == null)
                    throw new Exception($"Company with ID {request.CompanyId} not found");

                var departments = await _repo.GetByCompanyIdAsync(request.CompanyId);

                return departments.Select(d => new DepartmentDto(
                    Id: d.Id,
                    Code: d.Code,
                    CompanyId: d.CompanyId,
                    CompanyName: company.EngName ?? company.ArbName,
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