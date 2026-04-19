using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Groups.Dtos;
using Application.UARbac.Groups.Validators;
using Domain.UARbac;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Groups.Commands
{
    public static class CreateGroup
    {
        public record Command(CreateGroupDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateGroupValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IGroupRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IGroupRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var codeExists = await _repo.CodeExistsAsync(request.Data.Code);
                if (codeExists)
                    throw new ConflictException(_msg.CodeExists("Group", request.Data.Code));

                var group = new Group(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName
                );

                await _repo.AddAsync(group);
                await _repo.SaveChangesAsync(cancellationToken);

                return group.Id;
            }
        }
    }
}
