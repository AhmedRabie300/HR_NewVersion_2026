using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Education.Dtos;
using MediatR;

namespace Application.System.MasterData.Education.Queries
{
    public static class ListEducations
    {
        public record Query : IRequest<List<EducationDto>>;

        public class Handler : IRequestHandler<Query, List<EducationDto>>
        {
            private readonly IEducationRepository _repo;

            public Handler(IEducationRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<EducationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAllAsync();

                return items.Select(x => new EducationDto(
                    x.Id,
                    x.Code,
                    x.CompanyId,
                    x.Company?.EngName ?? x.Company?.ArbName,
                    x.EngName,
                    x.ArbName,
                    x.ArbName4S,
                    x.Level,
                    x.RequiredYears,
                    x.Remarks,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();
            }
        }
    }
}