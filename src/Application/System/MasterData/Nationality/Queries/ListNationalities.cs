using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Nationality.Dtos;
using MediatR;

namespace Application.System.MasterData.Nationality.Queries
{
    public static class ListNationalities
    {
        public record Query : IRequest<List<NationalityDto>>;

        public class Handler : IRequestHandler<Query, List<NationalityDto>>
        {
            private readonly INationalityRepository _repo;

            public Handler(INationalityRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<NationalityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var nationalities = await _repo.GetAllAsync();

                return nationalities.Select(n => new NationalityDto(
                    Id: n.Id,
                    Code: n.Code,
                    EngName: n.EngName,
                    ArbName: n.ArbName,
                    ArbName4S: n.ArbName4S,
                    IsMainNationality: n.IsMainNationality,
                    TravelRoute: n.TravelRoute,
                    TravelClass: n.TravelClass,
                    Remarks: n.Remarks,
                    TicketAmount: n.TicketAmount,
                    RegDate: n.RegDate,
                    CancelDate: n.CancelDate,
                    IsActive: n.IsActive()
                )).ToList();
            }
        }
    }
}