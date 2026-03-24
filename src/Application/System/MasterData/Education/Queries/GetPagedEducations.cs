using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Education.Dtos;
using MediatR;

namespace Application.System.MasterData.Education.Queries
{
    public static class GetPagedEducations
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm, int? CompanyId)
            : IRequest<PagedResult<EducationDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<EducationDto>>
        {
            private readonly IEducationRepository _repo;

            public Handler(IEducationRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<EducationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.CompanyId
                );

                var items = pagedResult.Items.Select(x => new EducationDto(
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

                return new PagedResult<EducationDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}