using Application.Common.Models;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using MediatR;
using Application.System.HRS.VacationAndEndOfService.VacationsType.Dtos;

namespace Application.System.HRS.VacationAndEndOfService.VacationsType.Queries
{
    public static class GetPagedVacationsTypes
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<VacationsTypeDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<VacationsTypeDto>>
        {
            private readonly IVacationsTypeRepository _repo;
            private readonly IContextService _contextService;

            public Handler(IVacationsTypeRepository repo, IContextService contextService)
            {
                _repo = repo;
                _contextService = contextService;
            }

            public async Task<PagedResult<VacationsTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(x => new VacationsTypeDto(
                    Id: x.Id,
                    Code: x.Code,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    IsPaid: x.IsPaid,
                    Sex: x.Sex,
                    IsAnnual: x.IsAnnual,
                    IsSickVacation: x.IsSickVacation,
                    IsFromAnnual: x.IsFromAnnual,
                    ForSalaryTransaction: x.ForSalaryTransaction,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName,
                    Remarks: x.Remarks,
                    OBalanceTransactionId: x.OBalanceTransactionId,
                    OverDueVacationId: x.OverDueVacationId,
                    Stage1Pct: x.Stage1Pct,
                    Stage2Pct: x.Stage2Pct,
                    Stage3Pct: x.Stage3Pct,
                    ForDeductionTransaction: x.ForDeductionTransaction,
                    AffectEos: x.AffectEos,
                    VactionTypeCaculation: x.VactionTypeCaculation,
                    ExceededDaysType: x.ExceededDaysType,
                    HasPayment: x.HasPayment,
                    RoundAnnualVacBalance: x.RoundAnnualVacBalance,
                    Religion: x.Religion,
                    IsOfficial: x.IsOfficial,
                    OverlapWithAnotherVac: x.OverlapWithAnotherVac,
                    ConsiderAllowedDays: x.ConsiderAllowedDays,
                    TimesNoInYear: x.TimesNoInYear,
                    AllowedDaysNo: x.AllowedDaysNo,
                    ExcludedFromSsRequests: x.ExcludedFromSsRequests,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();

                return new PagedResult<VacationsTypeDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}