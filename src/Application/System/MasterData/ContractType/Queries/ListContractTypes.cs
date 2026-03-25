using Application.System.MasterData.Abstractions;
using Application.System.MasterData.ContractType.Dtos;
using MediatR;

namespace Application.System.MasterData.ContractType.Queries
{
    public static class ListContractTypes
    {
        public record Query : IRequest<List<ContractTypeDto>>;

        public class Handler : IRequestHandler<Query, List<ContractTypeDto>>
        {
            private readonly IContractTypeRepository _repo;

            public Handler(IContractTypeRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<ContractTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var items = await _repo.GetAllAsync();

                return items.Select(x => new ContractTypeDto(
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
            }
        }
    }
}