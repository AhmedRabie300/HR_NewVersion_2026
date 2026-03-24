using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Religion.Dtos;
using MediatR;

namespace Application.System.MasterData.Religion.Queries
{
    public static class ListReligions
    {
        public record Query : IRequest<List<ReligionDto>>;

        public class Handler : IRequestHandler<Query, List<ReligionDto>>
        {
            private readonly IReligionRepository _repo;

            public Handler(IReligionRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<ReligionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var religions = await _repo.GetAllAsync();

                return religions.Select(r => new ReligionDto(
                    Id: r.Id,
                    Code: r.Code,
                    EngName: r.EngName,
                    ArbName: r.ArbName,
                    ArbName4S: r.ArbName4S,
                    Remarks: r.Remarks,
                    RegDate: r.RegDate,
                    CancelDate: r.CancelDate,
                    IsActive: r.IsActive()
                )).ToList();
            }
        }
    }
}