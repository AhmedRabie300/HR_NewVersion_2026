using Application.UARbac.Users.Dtos;
using Application.UARbac.Abstractions;
using Domain.UARbac;
using FluentValidation;
using MediatR;
using UserEntity = Domain.UARbac.Users;   

namespace Application.UARbac.Users.Commands
{
    public static class Create
    {
        public record Command(CreateUserDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data.Code)
                    .NotEmpty()
                    .MaximumLength(50)
                    .WithMessage("Code is required and must not exceed 50 characters");

                RuleFor(x => x.Data.Password)
                    .NotEmpty()
                    .WithMessage("Password is required");

                RuleFor(x => x.Data.EngName)
                    .MaximumLength(100);

                RuleFor(x => x.Data.ArbName)
                    .MaximumLength(100);
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IUserRepository _repo;

            public Handler(IUserRepository repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                 var existingUser = await _repo.GetByCodeAsync(request.Data.Code);
                if (existingUser != null)
                    throw new Exception($"User with code {request.Data.Code} already exists");

                 var user = new UserEntity();   

                var createdUser = await _repo.AddAsync(user);
                await _repo.SaveChangesAsync(cancellationToken);

                 if (request.Data.GroupIds != null && request.Data.GroupIds.Any())
                {
                    foreach (var groupId in request.Data.GroupIds)
                    {
                        await _repo.AddUserToGroupAsync(createdUser.Id, groupId, false);
                    }
                    await _repo.SaveChangesAsync(cancellationToken);
                }

                return createdUser.Id;
            }
        }
    }
}