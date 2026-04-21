using Application.Abstractions;
using Application.Common.Models;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalTransactions.Interval.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.FiscalTransactions.Interval.Queries
{
    public static class GetPagedIntervals
    {
        public record Query(
            int PageNumber = 1,
            int PageSize = 20,
            string? SearchTerm = null
        ) : IRequest<PagedResult<IntervalDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<IntervalDto>>
        {
            private readonly IIntervalRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IIntervalRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<PagedResult<IntervalDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items
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

                return new PagedResult<IntervalDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}