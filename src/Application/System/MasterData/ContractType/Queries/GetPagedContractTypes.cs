using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.ContractType.Dtos;
using MediatR;

namespace Application.System.MasterData.ContractType.Queries
{
    public static class GetPagedContractTypes
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm, int? CompanyId)
            : IRequest<PagedResult<ContractTypeDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<ContractTypeDto>>
        {
            private readonly IContractTypeRepository _repo;

            public Handler(IContractTypeRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<ContractTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.CompanyId
                );

                var items = pagedResult.Items.Select(x => new ContractTypeDto(
                    x.Id,
                    x.Code,
                    x.CompanyId,
                    x.Company?.EngName ?? x.Company?.ArbName,
                    x.EngName,
                    x.ArbName,
                    x.ArbName4S,
                    x.IsSpecial,
                    x.Remarks,
                    x.RegDate,
                    x.CancelDate,
                    x.IsActive()
                )).ToList();

                return new PagedResult<ContractTypeDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}