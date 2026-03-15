using Application.UARbac.ModulePermissions.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.ModulePermissions.Queries
{
    public static class GetModulePermissionsByModule
    {
        public record Query(int ModuleId) : IRequest<List<ModulePermissionDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.ModuleId).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, List<ModulePermissionDto>>
        {
            private readonly IModulePermissionRepository _repo;

            public Handler(IModulePermissionRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<ModulePermissionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var permissions = await _repo.GetByModuleIdAsync(request.ModuleId);

                return permissions.Select(p => new ModulePermissionDto(
                    Id: p.Id,
                    ModuleId: p.ModuleId,
                    ModuleCode: p.Module?.Code,
                    ModuleName: p.Module?.EngName ?? p.Module?.ArbName,
                    GroupId: p.GroupId,
                    GroupCode: p.Group?.Code,
                    GroupName: p.Group?.EngName ?? p.Group?.ArbName,
                    UserId: p.UserId,
                    UserCode: p.User?.Code,
                    UserName: p.User?.EngName ?? p.User?.ArbName,
                    CanView: p.CanView,
                    RegDate: p.RegDate,
                    CancelDate: p.CancelDate,
                    IsActive: p.IsActive()
                )).ToList();
            }
        }
    }
}