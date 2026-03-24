using Application.System.MasterData.Abstractions;
using Application.System.MasterData.MaritalStatus.Dtos;
using MediatR;

namespace Application.System.MasterData.MaritalStatus.Queries
{
    public static class ListMaritalStatuses
    {
        public record Query : IRequest<List<MaritalStatusDto>>;

        public class Handler : IRequestHandler<Query, List<MaritalStatusDto>>
        {
            private readonly IMaritalStatusRepository _repo;

            public Handler(IMaritalStatusRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<MaritalStatusDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAllAsync();

                return items.Select(x => new MaritalStatusDto(
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