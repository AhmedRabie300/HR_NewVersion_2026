using Application.System.MasterData.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using MediatR;

namespace Application.System.MasterData.BloodGroup.Queries
{
    public static class ListBloodGroups
    {
        public record Query : IRequest<List<BloodGroupDto>>;

        public class Handler : IRequestHandler<Query, List<BloodGroupDto>>
        {
            private readonly IBloodGroupRepository _repo;

            public Handler(IBloodGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<BloodGroupDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAllAsync();

                return items.Select(x => new BloodGroupDto(
                    x.Id,
                    x.Code,
                    x.EngName,
                    x.ArbName,
                    x.ArbName4S,
                    x.Remarks,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();
            }
        }
    }
}