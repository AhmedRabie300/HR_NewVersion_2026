using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Groups.Dtos;
using Application.UARbac.Groups.Validators;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Groups.Commands
{
    public static class UpdateGroup
    {
        public record Command(UpdateGroupDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateGroupValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IGroupRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IGroupRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var group = await _repo.GetByIdAsync(request.Data.Id);
                if (group == null)
                    throw new NotFoundException(_msg.NotFound("Group", request.Data.Id));

                group.Update(request.Data.EngName, request.Data.ArbName);

                await _repo.UpdateAsync(group);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
