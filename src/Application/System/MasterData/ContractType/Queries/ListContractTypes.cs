// Application/System/MasterData/ContractType/Queries/ListContractTypes.cs
using Application.Common.Abstractions;
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
            private readonly IContextService _ContextService;

            public Handler(IContractTypeRepository repo, IContextService ContextService)
            {
                _repo = repo;
                _ContextService = ContextService;
            }

            public async Task<List<ContractTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
              //  var companyId = _ContextService.GetCurrentCompanyId();

                var items = await _repo.GetAllAsync();

                return items.Select(x => new ContractTypeDto(
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
            }
        }
    }
}