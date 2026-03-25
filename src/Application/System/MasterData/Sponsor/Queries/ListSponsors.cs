using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using MediatR;

namespace Application.System.MasterData.Sponsor.Queries
{
    public static class ListSponsors
    {
        public record Query : IRequest<List<SponsorDto>>;

        public class Handler : IRequestHandler<Query, List<SponsorDto>>
        {
            private readonly ISponsorRepository _repo;

            public Handler(ISponsorRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<SponsorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAllAsync();

                return items.Select(x => new SponsorDto(
                    x.Id,
                    x.Code,
                    x.EngName,
                    x.ArbName,
                    x.ArbName4S,
                    x.SponsorNumber,
                    x.CompanyId,
                    x.Company?.EngName ?? x.Company?.ArbName,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();
            }
        }
    }
}