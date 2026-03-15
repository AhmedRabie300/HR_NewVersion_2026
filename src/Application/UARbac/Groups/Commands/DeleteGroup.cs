
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Groups.Commands
{
    public static class DeleteGroup
    {
        public record Command(int Id) : IRequest<bool>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Group ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IGroupRepository _repo;

            public Handler(IGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var exists = await _repo.ExistsAsync(request.Id);
                if (!exists)
                    return false;

                // Check if group has users
                var hasUsers = await _repo.HasUsersAsync(request.Id);
                if (hasUsers)
                    throw new Exception("Cannot delete group that has users assigned");

                await _repo.DeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}