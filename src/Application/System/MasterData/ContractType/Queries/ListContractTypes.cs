// Application/System/MasterData/ContractType/Queries/ListContractTypes.cs
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.ContractType.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.ContractType.Queries
{
    public static class ListContractTypes
    {
        public record Query : IRequest<List<ContractTypeDto>>;

        public class Handler : IRequestHandler<Query, List<ContractTypeDto>>
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
                    throw new UnauthorizedAccessException("Company ID is required in request header (X-CompanyId)");
                return companyId.Value;
            }

            public async Task<List<ContractTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();

                var items = await _repo.GetAllAsync(companyId);

                return items.Select(x => new ContractTypeDto(
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
            }
        }
    }
}