// Application/System/MasterData/Sector/Queries/GetPagedSectors.cs
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using MediatR;

namespace Application.System.MasterData.Sector.Queries
{
    public static class GetPagedSectors
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm, int? CompanyId, int Lang = 1) : IRequest<PagedResult<SectorDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<SectorDto>>
        {
            private readonly ISectorRepository _repo;

            public Handler(ISectorRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<SectorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.CompanyId
                );

                var items = pagedResult.Items.Select(s => new SectorDto(
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

                return new PagedResult<SectorDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}