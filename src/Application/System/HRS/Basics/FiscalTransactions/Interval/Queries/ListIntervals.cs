using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.Interval.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.Interval.Queries
{
    public static class ListIntervals
    {
        public record Query : IRequest<List<IntervalDto>>;

        public class Handler : IRequestHandler<Query, List<IntervalDto>>
        {
            private readonly IIntervalRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IIntervalRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<IntervalDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;

                var items = await _repo.GetAllAsync();

                return items
                    .Where(x => x.CompanyId == companyId)
                    .Select(x => new IntervalDto(
                        Id: x.Id,
                        Code: x.Code,
                        EngName: x.EngName,
                        ArbName: x.ArbName,
                        ArbName4S: x.ArbName4S,
                        Number: x.Number,
                        CompanyId: x.CompanyId,
                        Remarks: x.Remarks,
                        RegDate: x.RegDate,
                        CancelDate: x.CancelDate,
                        IsActive: x.IsActive()
                    )).ToList();
            }
        }
    }
}