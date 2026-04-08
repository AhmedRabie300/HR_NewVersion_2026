// Application/System/MasterData/ContractType/Queries/GetPagedContractTypes.cs
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
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IContractTypeRepository repo, IHttpContextAccessor httpContextAccessor)
            {
                _repo = repo;
                _httpContextAccessor = httpContextAccessor;
            }

            private int GetRequiredCompanyId()
            {
                var context = _httpContextAccessor.HttpContext;
                var companyId = context?.Items["CompanyId"] as int?;
                if (!companyId.HasValue)
                    throw new UnauthorizedAccessException("Company ID is required in request header");
                return companyId.Value;
            }

            public async Task<PagedResult<ContractTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    companyId
                );

                var items = pagedResult.Items.Select(x => new ContractTypeDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyId: x.CompanyId,
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