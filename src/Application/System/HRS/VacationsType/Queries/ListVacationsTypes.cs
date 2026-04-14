using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationsType.Dtos;
using MediatR;

namespace Application.System.HRS.VacationsType.Queries
{
    public static class ListVacationsTypes
    {
        public record Query : IRequest<List<VacationsTypeDto>>;

        public class Handler : IRequestHandler<Query, List<VacationsTypeDto>>
        {
            private readonly IVacationsTypeRepository _repo;
            private readonly IContextService _contextService;

            public Handler(IVacationsTypeRepository repo, IContextService contextService)
            {
                _repo = repo;
                _contextService = contextService;
            }

            public async Task<List<VacationsTypeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var items = await _repo.GetAllAsync();

                return items.Select(x => new VacationsTypeDto(
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
            }
        }
    }
}