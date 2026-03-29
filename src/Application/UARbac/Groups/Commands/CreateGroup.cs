using Application.UARbac.Groups.Dtos;
using Application.UARbac.Abstractions;
using Domain.UARbac;
using FluentValidation;
using MediatR;
using Application.Common;

namespace Application.UARbac.Groups.Commands
{
    public static class CreateGroup
    {
        public record Command(CreateGroupDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data.Code)
                    .NotEmpty()
                    .MaximumLength(50)
                    .WithMessage("Group code is required and must not exceed 50 characters");

                RuleFor(x => x.Data.EngName)
                    .MaximumLength(200)
                    .WithMessage("English name must not exceed 200 characters");

                RuleFor(x => x.Data.ArbName)
                    .MaximumLength(200)
                    .WithMessage("Arabic name must not exceed 200 characters");
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IGroupRepository _repo;

            public Handler(IGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                // Check if code exists
                var exists = await _repo.CodeExistsAsync(request.Data.Code);
                if (exists)
                    throw new ConflictException($"Group with code '{request.Data.Code}' already exists");

                // Create group
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