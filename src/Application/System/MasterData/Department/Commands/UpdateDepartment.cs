using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Department.Dtos;
using Application.System.MasterData.Department.Validators;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Department.Commands
{
    public static class UpdateDepartment
    {
        public record Command(UpdateDepartmentDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IDepartmentRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateDepartmentValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDepartmentRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IDepartmentRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Department", request.Data.Id));

                 entity.UpdateBasicInfo(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks,
                    request.Data.CostCenterCode
                );

                 if (request.Data.ParentId.HasValue && request.Data.ParentId != entity.Id)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new NotFoundException(_msg.NotFound("ParentDepartment", request.Data.ParentId.Value));

                    entity.UpdateParent(request.Data.ParentId);
                }

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}