using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Bank.Dtos;
using MediatR;

namespace Application.System.MasterData.Bank.Queries
{
    public static class ListBanks
    {
        public record Query : IRequest<List<BankDto>>;

        public class Handler : IRequestHandler<Query, List<BankDto>>
        {
            private readonly IBankRepository _repo;
 
            public Handler(IBankRepository repo )
            {
                _repo = repo;
             }

            public async Task<List<BankDto>> Handle(Query request, CancellationToken cancellationToken)
            {
 
                var items = await _repo.GetAllAsync();

                return items.Select(x => new BankDto(
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
            }
        }
    }
}