using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Document.Dtos;
using MediatR;

namespace Application.System.MasterData.Document.Queries
{
    public static class GetPagedDocuments
    {
        public record Query(
            int PageNumber,
            int PageSize,
            string? SearchTerm
        ) : IRequest<PagedResult<DocumentDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<DocumentDto>>
        {
            private readonly IDocumentRepository _repo;

            public Handler(IDocumentRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<DocumentDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                // ✅ تمرير null كـ groupId (أو القيمة المناسبة)
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    null  // ✅ groupId (ممكن يكون null أو قيمة معينة)
                );

                var items = pagedResult.Items.Select(x => new DocumentDto(
                    x.Id,
                    x.Code,
                    x.EngName,
                    x.ArbName,
                    x.ArbName4S,
                    x.IsForCompany,
                    x.Remarks,
                    x.DocumentTypesGroupId,
                    x.DocumentTypesGroup?.EngName ?? x.DocumentTypesGroup?.ArbName,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();

                return new PagedResult<DocumentDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}