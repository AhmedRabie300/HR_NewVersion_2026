using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Groups.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Groups.Queries
{
    public static class GetGroupWithUsers
    {
        public record Query(int Id) : IRequest<GroupWithUsersDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, GroupWithUsersDto>
        {
            private readonly IGroupRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IGroupRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<GroupWithUsersDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var group = await _repo.GetByIdAsync(request.Id);
                if (group == null)
                    throw new NotFoundException(_msg.NotFound("Group", request.Id));

                var userGroups = await _repo.GetGroupUsersAsync(request.Id);

                var users = userGroups.Select(ug => new GroupUserDto(
                    UserId: ug.UserId,
                    UserCode: ug.User?.Code ?? string.Empty,
                    UserName: ug.User?.EngName ?? ug.User?.ArbName ?? string.Empty
                )).ToList();

                return new GroupWithUsersDto(
                    Id: group.Id,
                    Code: group.Code,
                    EngName: group.EngName,
                    ArbName: group.ArbName,
                    RegDate: group.RegDate,
                    CancelDate: group.CancelDate,
                    UsersCount: users.Count,
                    Users: users
                );
            }
        }
    }
}
