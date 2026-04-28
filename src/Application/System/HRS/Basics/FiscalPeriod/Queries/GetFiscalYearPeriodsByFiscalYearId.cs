using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalPeriod.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.FiscalPeriod.Queries
{
    public static class GetFiscalYearPeriodsByFiscalYearId
    {
        public record Query(int FiscalYearId) : IRequest<List<FiscalYearPeriodDto>>;

        public class Handler : IRequestHandler<Query, List<FiscalYearPeriodDto>>
        {
            private readonly IFiscalYearPeriodRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IFiscalYearPeriodRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<FiscalYearPeriodDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var items = await _repo.GetByFiscalYearIdAsync(request.FiscalYearId);

                return items.Select(x => new FiscalYearPeriodDto(
                    Id: x.Id,
                    Code: x.Code,
                    FiscalYearId: x.FiscalYearId,
                    FiscalYearName: lang == 2 ? x.FiscalYear?.ArbName : x.FiscalYear?.EngName,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    FromDate: x.FromDate,
                    ToDate: x.ToDate,
                    Remarks: x.Remarks,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    HFromDate: x.HFromDate,
                    HToDate: x.HToDate,
                    PeriodType: x.PeriodType,
                    PeriodRank: x.PeriodRank,
                    PrepareFromDate: x.PrepareFromDate,
                    PrepareToDate: x.PrepareToDate,
                    CompanyId: x.CompanyId,
                    CompanyName: lang == 2 ? x.Company?.ArbName : x.Company?.EngName,
                    IsActive: x.IsActive(),
                    Modules: x.Modules.Select(m => new FiscalYearPeriodModuleDto(
                        Id: m.Id,
                        FiscalYearPeriodId: m.FiscalYearPeriodId,
                        ModuleId: m.ModuleId,
                        ModuleName: lang == 2 ? m.Module?.ArbName : m.Module?.EngName,
                        OpenDate: m.OpenDate,
                        CloseDate: m.CloseDate,
                        Remarks: m.Remarks,
                        RegDate: m.RegDate,
                        CancelDate: m.CancelDate,
                        IsActive: m.IsActive(),
                        IsOpen: m.IsOpen,
                        IsClosed: m.IsClosed
                    )).ToList()
                )).ToList();
            }
        }
    }
}