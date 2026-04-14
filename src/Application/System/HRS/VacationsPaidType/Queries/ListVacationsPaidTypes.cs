using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationsPaidType.Dtos;
using MediatR;

namespace Application.System.HRS.VacationsPaidType.Queries
{
    public static class ListVacationsPaidTypes
    {
        public record Query : IRequest<List<VacationsPaidTypeDto>>;

        public class Handler : IRequestHandler<Query, List<VacationsPaidTypeDto>>
        {
            private readonly IVacationsPaidTypeRepository _repo;

            public Handler(IVacationsPaidTypeRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<VacationsPaidTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAllAsync();

                return items.Select(x => new VacationsPaidTypeDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}