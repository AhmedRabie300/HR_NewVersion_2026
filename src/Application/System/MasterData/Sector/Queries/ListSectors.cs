using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using MediatR;

namespace Application.System.MasterData.Sector.Queries
{
    public static class ListSectors
    {
        public record Query(int Lang = 1) : IRequest<List<SectorDto>>;

        public class Handler : IRequestHandler<Query, List<SectorDto>>
        {
            private readonly ISectorRepository _repo;

            public Handler(ISectorRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<SectorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var sectors = await _repo.GetAllAsync();

                return sectors.Select(s => new SectorDto(
                    Id: s.Id,
                    Code: s.Code,
                    CompanyId: s.CompanyId,
                    CompanyName: request.Lang == 2 ? s.Company?.ArbName : s.Company?.EngName,
                    EngName: s.EngName,
                    ArbName: s.ArbName,
                    ArbName4S: s.ArbName4S,
                    ParentId: s.ParentId,
                    ParentSectorName: request.Lang == 2 ? s.ParentSector?.ArbName : s.ParentSector?.EngName,
                    Remarks: s.Remarks,
                    RegDate: s.RegDate,
                    CancelDate: s.CancelDate,
                    IsActive: s.IsActive()
                )).ToList();
            }
        }
    }
}