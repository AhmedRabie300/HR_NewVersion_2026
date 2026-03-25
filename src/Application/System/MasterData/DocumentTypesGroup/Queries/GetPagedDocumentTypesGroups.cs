using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using MediatR;

namespace Application.System.MasterData.DocumentTypesGroup.Queries
{
    public static class GetPagedDocumentTypesGroups
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<DocumentTypesGroupDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<DocumentTypesGroupDto>>
        {
            private readonly IDocumentTypesGroupRepository _repo;

            public Handler(IDocumentTypesGroupRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<DocumentTypesGroupDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new DocumentTypesGroupDto(
                    x.Id,
                    x.Code,
                    x.EngName,
                    x.ArbName,
                    x.Remarks,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();

                return new PagedResult<DocumentTypesGroupDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}