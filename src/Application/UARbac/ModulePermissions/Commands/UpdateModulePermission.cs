using Application.UARbac.ModulePermissions.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;
using Application.Common;

namespace Application.UARbac.ModulePermissions.Commands
{
    public static class UpdateModulePermission
    {
        public record Command(UpdateModulePermissionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data.Id).GreaterThan(0);
                RuleFor(x => x.Data)
                    .Must(x => x.CanView.HasValue)
                    .WithMessage("At least one field must be provided");
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IModulePermissionRepository _repo;

            public Handler(IModulePermissionRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var permission = await _repo.GetByIdAsync(request.Data.Id);
                if (permission == null)
                    throw new NotFoundException("NotFound", $"Permission with ID {request.Data.Id} not found");

                permission.UpdatePermission(request.Data.CanView);

                await _repo.UpdateAsync(permission);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}