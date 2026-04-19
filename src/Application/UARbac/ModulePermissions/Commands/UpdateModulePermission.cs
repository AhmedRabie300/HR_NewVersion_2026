using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.ModulePermissions.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.ModulePermissions.Commands
{
    public static class UpdateModulePermission
    {
        public record Command(UpdateModulePermissionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data.Id)
                    .GreaterThan(0)
                    .WithMessage(msg.Get("IdGreaterThanZero"));

                RuleFor(x => x.Data)
                    .Must(x => x.CanView.HasValue)
                    .WithMessage(msg.Get("AtLeastOneField"));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IModulePermissionRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IModulePermissionRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var permission = await _repo.GetByIdAsync(request.Data.Id);
                if (permission == null)
                    throw new NotFoundException(_msg.NotFound("ModulePermission", request.Data.Id));

                permission.UpdatePermission(request.Data.CanView);

                await _repo.UpdateAsync(permission);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
