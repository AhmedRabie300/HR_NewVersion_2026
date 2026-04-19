using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.ModulePermissions.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.ModulePermissions.Queries
{
    public static class GetModulePermissionById
    {
        public record Query(int Id) : IRequest<ModulePermissionDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, ModulePermissionDto>
        {
            private readonly IModulePermissionRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IModulePermissionRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<ModulePermissionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var permission = await _repo.GetByIdAsync(request.Id);
                if (permission == null)
                    throw new NotFoundException(_msg.NotFound("ModulePermission", request.Id));

                return new ModulePermissionDto(
                    Id: permission.Id,
                    ModuleId: permission.ModuleId,
                    ModuleCode: permission.Module?.Code,
                    ModuleName: permission.Module?.EngName ?? permission.Module?.ArbName,
                    GroupId: permission.GroupId,
                    GroupCode: permission.Group?.Code,
                    GroupName: permission.Group?.EngName ?? permission.Group?.ArbName,
                    UserId: permission.UserId,
                    UserCode: permission.User?.Code,
                    UserName: permission.User?.EngName ?? permission.User?.ArbName,
                    CanView: permission.CanView,
                    RegDate: permission.RegDate,
                    CancelDate: permission.CancelDate,
                    IsActive: permission.IsActive()
                );
            }
        }
    }
}
