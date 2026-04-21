using Application.Common.Models;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using MediatR;
using Application.System.HRS.Basics.FiscalTransactions.TransactionsGroup.Dtos;

namespace Application.System.HRS.Basics.FiscalTransactions.TransactionsGroup.Queries
{
    public static class GetPagedTransactionsGroups
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<TransactionsGroupDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<TransactionsGroupDto>>
        {
            private readonly ITransactionsGroupRepository _repo;
            private readonly IContextService _contextService;

            public Handler(ITransactionsGroupRepository repo, IContextService contextService)
            {
                _repo = repo;
                _contextService = contextService;
            }

            public async Task<PagedResult<TransactionsGroupDto>> Handle(Query request, CancellationToken cancellationToken)
            {
 
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new TransactionsGroupDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName,
                    Remarks: x.Remarks,
                    RegComputerId: x.RegComputerId,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();

                return new PagedResult<TransactionsGroupDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}