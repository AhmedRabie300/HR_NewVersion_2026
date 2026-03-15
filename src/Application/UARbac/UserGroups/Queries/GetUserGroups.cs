// Application/UARbac/UserGroups/Queries/GetUserGroups.cs
using Application.UARbac.UserGroups.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.UserGroups.Queries
{
    public static class GetUserGroups
    {
        public record Query(int UserId) : IRequest<List<UserGroupDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, List<UserGroupDto>>
        {
            private readonly IUserGroupRepository _repo;

            public Handler(IUserGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<UserGroupDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userGroups = await _repo.GetByUserIdAsync(request.UserId);

                return userGroups.Select(ug => new UserGroupDto(
                                     ug.Id,
    ug.Group?.Code ?? "",
    ug.UserId,
    ug.User?.Code ?? "",
    ug.User?.EngName ?? ug.User?.ArbName,
    ug.GroupId,
    ug.Group?.Code ?? "",
    ug.Group?.EngName ?? ug.Group?.ArbName
 
)).ToList();
            }
        }
    }
}