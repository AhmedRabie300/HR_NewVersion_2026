using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Profession.Dtos;
using MediatR;

namespace Application.System.MasterData.Profession.Queries
{
    public static class GetPagedProfessions
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm, int? CompanyId)
            : IRequest<PagedResult<ProfessionDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<ProfessionDto>>
        {
            private readonly IProfessionRepository _repo;

            public Handler(IProfessionRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<ProfessionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.CompanyId
                );

                var items = pagedResult.Items.Select(x => new ProfessionDto(
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

                return new PagedResult<ProfessionDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}