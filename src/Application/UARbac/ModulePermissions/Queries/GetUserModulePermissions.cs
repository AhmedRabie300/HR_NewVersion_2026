using Application.UARbac.ModulePermissions.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.ModulePermissions.Queries
{
    public static class GetUserModulePermissions
    {
        public record Query(int UserId) : IRequest<List<UserModulePermissionDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, List<UserModulePermissionDto>>
        {
            private readonly IModulePermissionRepository _repo;

            public Handler(IModulePermissionRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<UserModulePermissionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _repo.GetUserModulePermissionsAsync(request.UserId);
            }
        }
    }
}