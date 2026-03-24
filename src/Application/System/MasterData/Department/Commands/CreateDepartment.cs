using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using Application.System.MasterData.Department.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Department.Commands
{
    public static class CreateDepartment
    {
        public record Command(CreateDepartmentDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateDepartmentValidator());
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IDepartmentRepository _repo;
            private readonly ICompanyRepository _companyRepo;

            public Handler(IDepartmentRepository repo, ICompanyRepository companyRepo)
            {
                _repo = repo;
                _companyRepo = companyRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                // Check if company exists
                var company = await _companyRepo.GetByIdAsync(request.Data.CompanyId);
                if (company == null)
                    throw new Exception($"Company with ID {request.Data.CompanyId} not found");

                // Check if code exists within the same company
                var exists = await _repo.CodeExistsAsync(request.Data.Code, request.Data.CompanyId);
                if (exists)
                    throw new Exception($"Department with code '{request.Data.Code}' already exists in this company");

                // Check if parent department exists if provided
                if (request.Data.ParentId.HasValue)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new Exception($"Parent department with ID {request.Data.ParentId} not found");
                }

                var department = new Domain.System.MasterData.Department(
                    code: request.Data.Code,
                    companyId: request.Data.CompanyId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    parentId: request.Data.ParentId,
                    remarks: request.Data.Remarks,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.regComputerId,
                    costCenterCode: request.Data.CostCenterCode
                );

                await _repo.AddAsync(department);
                await _repo.SaveChangesAsync(cancellationToken);

                return department.Id;
            }
        }
    }
}