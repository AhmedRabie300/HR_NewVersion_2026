using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Sponsor.Queries
{
    public static class ListSponsors
    {
        public record Query : IRequest<List<SponsorDto>>;

        public class Handler : IRequestHandler<Query, List<SponsorDto>>
        {
            private readonly ISponsorRepository _repo;
            private readonly IContextService _ContextService;

            public Handler(
                ISponsorRepository repo,
                IContextService ContextService
                )
            {
                _repo = repo;
                _ContextService = ContextService;
            }

            public async Task<List<SponsorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                 var items = await _repo.GetAllAsync();

                return items.Select(x => new SponsorDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    SponsorNumber: x.SponsorNumber,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}