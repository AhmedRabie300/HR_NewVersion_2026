using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Users.Dtos;
using Application.UARbac.Users.Validators;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Users.Commands
{
    public static class Update
    {
        public record Command(UpdateUserDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateUserValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUserRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IUserRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _repo.GetByIdAsync(request.Data.Id);
                if (user == null)
                    throw new NotFoundException(_msg.NotFound("User", request.Data.Id));

                if (request.Data.EngName != null)
                    user.UpdatePersonalInfo(request.Data.EngName, null, null, null);

                if (request.Data.ArbName != null)
                    user.UpdatePersonalInfo(null, request.Data.ArbName, null, null);

                if (request.Data.IsAdmin != null)
                    user.UpdatePermissions(request.Data.IsAdmin, null, null);

                if (request.Data.DeviceToken != null)
                    user.UpdateDeviceToken(request.Data.DeviceToken);

                await _repo.UpdateAsync(user);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
