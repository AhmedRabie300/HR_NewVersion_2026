using Application.Common;
using Application.Common.Abstractions;
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
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IGroupRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IGroupRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var exists = await _repo.ExistsAsync(request.Id);
                if (!exists)
                    throw new NotFoundException(_msg.NotFound("Group", request.Id));

                var hasUsers = await _repo.HasUsersAsync(request.Id);
                if (hasUsers)
                    throw new ConflictException(_msg.Get("CannotDeleteHasChildren"));

                await _repo.DeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}
