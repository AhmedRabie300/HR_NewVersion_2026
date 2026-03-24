using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DependantType.Dtos;
using MediatR;

namespace Application.System.MasterData.DependantType.Queries
{
    public static class ListDependantTypes
    {
        public record Query : IRequest<List<DependantTypeDto>>;

        public class Handler : IRequestHandler<Query, List<DependantTypeDto>>
        {
            private readonly IDependantTypeRepository _repo;

            public Handler(IDependantTypeRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<DependantTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAllAsync();

                return items.Select(x => new DependantTypeDto(
                    x.Id,
                    x.Code,
                    x.CompanyId,
                    x.Company?.EngName ?? x.Company?.ArbName,
                    x.EngName,
                    x.ArbName,
                    x.ArbName4S,
                    x.Remarks,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();
            }
        }
    }
}