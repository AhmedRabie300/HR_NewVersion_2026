// Application/System/MasterData/Department/Queries/GetDepartmentById.cs
using Application.Common;
using Application.Common.Abstractions;
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
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, DepartmentDto>
        {
            private readonly IDepartmentRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                IDepartmentRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<DepartmentDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Department", request.Id));

                return new DepartmentDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    CompanyId: entity.CompanyId,
                    CompanyName: entity.Company?.EngName ?? entity.Company?.ArbName,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    ParentId: entity.ParentId,
                    ParentDepartmentName: entity.ParentDepartment?.EngName ?? entity.ParentDepartment?.ArbName,
                    Remarks: entity.Remarks,
                    CostCenterCode: entity.CostCenterCode,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}