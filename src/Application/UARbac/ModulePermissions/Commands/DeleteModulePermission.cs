using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.ModulePermissions.Commands
{
    public static class DeleteModulePermission
    {
        public record Command(int Id) : IRequest<bool>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IModulePermissionRepository _repo;

            public Handler(IModulePermissionRepository repo)
            {
                _repo = repo;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var exists = await _repo.ExistsAsync(request.Id);
                if (!exists)
                    return false;

                await _repo.DeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}