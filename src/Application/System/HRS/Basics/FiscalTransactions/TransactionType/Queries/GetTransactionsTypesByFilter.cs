using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.TransactionType.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.TransactionType.Queries
{
    public static class GetTransactionsTypesByFilter
    {
        public record Query(int? TransactionGroupId = null, short? Sign = null) : IRequest<List<TransactionsTypeBasicDto>>;

        public class Handler : IRequestHandler<Query, List<TransactionsTypeBasicDto>>
        {
            private readonly ITransactionsTypeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(ITransactionsTypeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<TransactionsTypeBasicDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;

                var allItems = await _repo.GetByCompanyIdAsync(companyId);

                var filteredItems = allItems.AsEnumerable();

                if (request.TransactionGroupId.HasValue && request.TransactionGroupId.Value > 0)
                {
                    filteredItems = filteredItems.Where(x => x.TransactionGroupId == request.TransactionGroupId.Value);
                }

                if (request.Sign.HasValue)
                {   
                    filteredItems = filteredItems.Where(x => x.Sign == request.Sign.Value);
                }

                return filteredItems.Select(x => new TransactionsTypeBasicDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName
                )).ToList();
            }
        }
    }
}