// Application/UARbac/Groups/Queries/ListGroups.cs
using Application.UARbac.Groups.Dtos;
using Application.UARbac.Abstractions;
using MediatR;

namespace Application.UARbac.Groups.Queries
{
    public static class ListGroups
    {
        public record Query : IRequest<List<GroupDto>>;

        public class Handler : IRequestHandler<Query, List<GroupDto>>
        {
            private readonly IGroupRepository _repo;

            public Handler(IGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<GroupDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var groups = await _repo.GetAllAsync();

                return groups.Select(group => new GroupDto(
                    group.Id,
                    group.Code,
                    group.EngName,
                    group.ArbName,
                    group.RegDate,
                    group.CancelDate
                )).ToList();
            }
        }
    }
}