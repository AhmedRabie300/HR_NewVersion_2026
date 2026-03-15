using Application.UARbac.Groups.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Groups.Queries
{
    public static class GetGroupById
    {
        public record Query(int Id) : IRequest<GroupDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Group ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, GroupDto>
        {
            private readonly IGroupRepository _repo;

            public Handler(IGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<GroupDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var group = await _repo.GetByIdAsync(request.Id);
                if (group == null)
                    throw new Exception($"Group with ID {request.Id} not found");

                return new GroupDto(
                    group.Id,
                    group.Code,
                    group.EngName,
                    group.ArbName,
                    group.RegDate,
                    group.CancelDate
                );
            }
        }
    }
}