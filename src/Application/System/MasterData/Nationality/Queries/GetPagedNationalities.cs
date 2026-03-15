using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Nationality.Dtos;
using MediatR;

namespace Application.System.MasterData.Nationality.Queries
{
    public static class GetPagedNationalities
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm) : IRequest<PagedResult<NationalityDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<NationalityDto>>
        {
            private readonly INationalityRepository _repo;

            public Handler(INationalityRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<NationalityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(n => new NationalityDto(
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

                return new PagedResult<NationalityDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}