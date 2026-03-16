using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using Application.System.MasterData.Department.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Department.Commands
{
    public static class UpdateDepartment
    {
        public record Command(UpdateDepartmentDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateDepartmentValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDepartmentRepository _repo;

            public Handler(IDepartmentRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var department = await _repo.GetByIdAsync(request.Data.Id);
                if (department == null)
                    throw new Exception($"Department with ID {request.Data.Id} not found");

                // Update basic info
                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null ||
                    request.Data.CostCenterCode != null)
                {
                    department.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks,
                        request.Data.CostCenterCode
                    );
                }

                // Update parent
                if (request.Data.ParentId.HasValue)
                {
                    // Check if parent exists and not self-reference
                    if (request.Data.ParentId != department.Id)
                    {
                        var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                        if (parent == null)
                            throw new Exception($"Parent department with ID {request.Data.ParentId} not found");

                        department.UpdateParent(request.Data.ParentId);
                    }
                }

                await _repo.UpdateAsync(department);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}