using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Profession.Dtos;
using MediatR;

namespace Application.System.MasterData.Profession.Queries
{
    public static class ListProfessions
    {
        public record Query : IRequest<List<ProfessionDto>>;

        public class Handler : IRequestHandler<Query, List<ProfessionDto>>
        {
            private readonly IProfessionRepository _repo;

            public Handler(IProfessionRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<ProfessionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAllAsync();

                return items.Select(x => new ProfessionDto(
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