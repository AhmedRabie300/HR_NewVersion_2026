using Application.UARbac.Groups.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Groups.Queries
{
    public static class GetGroupWithUsers
    {
        public record Query(int Id) : IRequest<GroupWithUsersDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, GroupWithUsersDto>
        {
            private readonly IGroupRepository _repo;

            public Handler(IGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<GroupWithUsersDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var group = await _repo.GetByIdAsync(request.Id);
                if (group == null)
                    throw new Exception($"Group with ID {request.Id} not found");

                var userGroups = await _repo.GetGroupUsersAsync(request.Id);

                var users = userGroups.Select(ug => new GroupUserDto(
                    ug.UserId,
                    ug.User?.Code ?? "",
                    ug.User?.EngName ?? ug.User?.ArbName ?? ""
                  )).ToList();

                return new GroupWithUsersDto(
                    group.Id,
                    group.Code,
                    group.EngName,
                    group.ArbName,
                    group.RegDate,
                    group.CancelDate,
                    users.Count,
                    users
                );
            }
        }
    }
}