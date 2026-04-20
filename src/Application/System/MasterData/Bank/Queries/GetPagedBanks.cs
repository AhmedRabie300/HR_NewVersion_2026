using Application.Common.Models;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Bank.Dtos;
using MediatR;

namespace Application.System.MasterData.Bank.Queries
{
    public static class GetPagedBanks
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<BankDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<BankDto>>
        {
            private readonly IBankRepository _repo;

            public Handler(IBankRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<BankDto>> Handle(Query request, CancellationToken cancellationToken)
            {

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new BankDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();

                return new PagedResult<BankDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}