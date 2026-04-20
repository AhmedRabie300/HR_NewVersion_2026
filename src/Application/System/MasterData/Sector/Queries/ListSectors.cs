// Application/System/MasterData/Sector/Queries/ListSectors.cs
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Sector.Queries
{
    public static class ListSectors
    {
        public record Query : IRequest<List<SectorDto>>;

        public class Handler : IRequestHandler<Query, List<SectorDto>>
        {
            private readonly ISectorRepository _repo;
            private readonly IContextService _ContextService;

            public Handler(ISectorRepository repo,  IContextService ContextService)
            {
                _repo = repo;
                _ContextService = ContextService;
            }

            public async Task<List<SectorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                 var items = await _repo.GetAllAsync();

                return items.Select(x => new SectorDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyId: x.CompanyId,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    ParentId: x.ParentId,
                    ParentSectorName: x.ParentSector?.EngName ?? x.ParentSector?.ArbName,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}