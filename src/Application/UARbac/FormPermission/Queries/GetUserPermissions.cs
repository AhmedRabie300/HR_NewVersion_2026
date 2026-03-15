// Application/UARbac/Permissions/Queries/GetUserPermissions.cs
using Application.UARbac.Abstractions;
using Application.UARbac.FormPermission.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.FormPermission.Queries
{
    public static class GetUserPermissions
    {
        public record Query(int UserId) : IRequest<List<UserFormPermissionDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, List<UserFormPermissionDto>>
        {
            private readonly IFormPermissionRepository _repo;

            public Handler(IFormPermissionRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<UserFormPermissionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var permissions = await _repo.GetUserEffectivePermissionsAsync(request.UserId);

                // Group by FormId and merge permissions
                var groupedPermissions = permissions
                    .GroupBy(x => x.FormId)
                    .Select(g => new UserFormPermissionDto(
                        g.Key,
                        g.First().Form?.Code ?? "",
                        g.First().Form?.EngName ?? g.First().Form?.ArbName ?? "",
                        g.Any(x => x.AllowView == true),
                        g.Any(x => x.AllowAdd == true),
                        g.Any(x => x.AllowEdit == true),
                        g.Any(x => x.AllowDelete == true),
                        g.Any(x => x.AllowPrint == true),
                        g.Count() > 1 ? "Multiple" : (g.First().UserId.HasValue ? "User" : "Group")
                    ))
                    .ToList();

                return groupedPermissions;
            }
        }
    }
}