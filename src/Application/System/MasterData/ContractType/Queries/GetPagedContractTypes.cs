// Application/System/MasterData/ContractType/Queries/GetPagedContractTypes.cs
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.ContractType.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.ContractType.Queries
{
    public static class GetPagedContractTypes
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<ContractTypeDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<ContractTypeDto>>
        {
            private readonly IContractTypeRepository _repo;
            private readonly IContextService _ContextService;

            public Handler(IContractTypeRepository repo, IContextService ContextService)
            {
                _repo = repo;
                _ContextService = ContextService;
            }

            public async Task<PagedResult<ContractTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //var companyId = _ContextService.GetCurrentCompanyId();

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                     
                );

                var items = pagedResult.Items.Select(x => new ContractTypeDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyName: x.Company?.EngName ?? x.Company?.ArbName,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    IsSpecial: x.IsSpecial,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
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