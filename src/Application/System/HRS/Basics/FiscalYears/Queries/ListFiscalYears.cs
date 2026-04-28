using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalYears.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.FiscalYears.Queries
{
    public static class ListFiscalYears
    {
        public record Query : IRequest<List<FiscalYearDto>>;

        public class Handler : IRequestHandler<Query, List<FiscalYearDto>>
        {
            private readonly IFiscalYearRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IFiscalYearRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<FiscalYearDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var items = await _repo.GetByCompanyIdAsync();

                return items.Select(x => new FiscalYearDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    CompanyId: x.CompanyId,
                    CompanyName: lang == 2 ? x.Company?.ArbName : x.Company?.EngName,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}