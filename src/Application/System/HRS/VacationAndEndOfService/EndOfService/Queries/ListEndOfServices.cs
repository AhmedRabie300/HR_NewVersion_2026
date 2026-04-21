using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.EndOfService.Dtos;
using MediatR;

namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Queries
{
    public static class ListEndOfServices
    {
        public record Query : IRequest<List<EndOfServiceDto>>;

        public class Handler : IRequestHandler<Query, List<EndOfServiceDto>>
        {
            private readonly IEndOfServiceRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IEndOfServiceRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<EndOfServiceDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var companyId = _currentUser.CompanyId;

                var items = await _repo.GetByCompanyIdAsync(companyId);

                return items.Select(x => new EndOfServiceDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    CompanyId: x.CompanyId,
                    CompanyName: lang == 2 ? x.Company?.ArbName : x.Company?.EngName,
                    Remarks: x.Remarks,
                    ExtraTransactionId: x.ExtraTransactionId,
                    ExcludedFromSSRequests: x.ExcludedFromSSRequests,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive(),
                    Rules: x.Rules.Select(r => new EndOfServiceRuleDto(
                        Id: r.Id,
                        EndOfServiceId: r.EndOfServiceId,
                        FromWorkingMonths: r.FromWorkingMonths,
                        ToWorkingMonths: r.ToWorkingMonths,
                        AmountPercent: r.AmountPercent,
                        Formula: r.Formula,
                        ExtraDedFormula: r.ExtraDedFormula,
                        Remarks: r.Remarks,
                        RegDate: r.RegDate,
                        CancelDate: r.CancelDate,
                        IsActive: r.IsActive()
                    )).ToList()
                )).ToList();
            }
        }
    }
}