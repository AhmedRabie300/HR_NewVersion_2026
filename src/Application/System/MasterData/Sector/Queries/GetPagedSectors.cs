// Application/System/MasterData/Sector/Queries/GetPagedSectors.cs
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Sector.Queries
{
    public static class GetPagedSectors
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<SectorDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<SectorDto>>
        {
            private readonly ISectorRepository _repo;
            private readonly IContextService _ContextService;

            public Handler(ISectorRepository repo,  IContextService ContextService)
            {
                _repo = repo;
                _ContextService = ContextService;
            }

            public async Task<PagedResult<SectorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                 var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm 
                );

                var items = pagedResult.Items.Select(x => new SectorDto(
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