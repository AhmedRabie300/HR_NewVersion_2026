using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.TransactionsGroup.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.TransactionsGroup.Queries
{
    public static class GetTransactionsGroupsByCompanyId
    {
        public record Query(int CompanyId) : IRequest<List<TransactionsGroupDto>>;

        public class Handler : IRequestHandler<Query, List<TransactionsGroupDto>>
        {
            private readonly ITransactionsGroupRepository _repo;
            private readonly IContextService _contextService;

            public Handler(ITransactionsGroupRepository repo, IContextService contextService)
            {
                _repo = repo;
                _contextService = contextService;
            }

            public async Task<List<TransactionsGroupDto>> Handle(Query request, CancellationToken cancellationToken)
            {
 
                var items = await _repo.GetByCompanyIdAsync();

                return items.Select(x => new TransactionsGroupDto(
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
            }
        }
    }
}