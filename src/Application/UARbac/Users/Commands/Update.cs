// Application/UARbac/Users/Commands/Update.cs
using Application.UARbac.Users.Dtos;
using Application.UARbac.Abstractions;
using Domain.UARbac;
using FluentValidation;
using MediatR;
using Mapster;

namespace Application.UARbac.Users.Commands
{
    public static class Update
    {
        public record Command(UpdateUserDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data.Id)
                    .GreaterThan(0)
                    .WithMessage("User Id is required.");

                RuleFor(x => x.Data.EngName)
                    .MaximumLength(100)
                    .WithMessage("English name max length is 100.");

                RuleFor(x => x.Data.ArbName)
                    .MaximumLength(100)
                    .WithMessage("Arabic name max length is 100.");

                // At least one field must be provided
                RuleFor(x => x.Data)
                    .Must(x => x.EngName != null ||
                               x.ArbName != null ||
                               x.IsAdmin != null ||
                               x.DeviceToken != null)
                    .WithMessage("At least one field must be provided to update.");
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUserRepository _repo;

            public Handler(IUserRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // Get user by id
                var user = await _repo.GetByIdAsync(request.Data.Id);
                if (user == null)
                    throw new Exception($"User with ID {request.Data.Id} not found");

                // Update user properties
                if (request.Data.EngName != null)
                    user.UpdatePersonalInfo(request.Data.EngName, null, null, null);

                if (request.Data.ArbName != null)
                    user.UpdatePersonalInfo(null, request.Data.ArbName, null, null);

                if (request.Data.IsAdmin != null)
                    user.UpdatePermissions(request.Data.IsAdmin, null, null);

                if (request.Data.DeviceToken != null)
                    user.UpdateDeviceToken(request.Data.DeviceToken);

                // Save changes
                await _repo.UpdateAsync(user);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}