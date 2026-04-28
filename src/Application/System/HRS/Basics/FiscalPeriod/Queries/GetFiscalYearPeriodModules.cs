using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalPeriod.Dtos;
using MediatR;

namespace Application.System.HRS.Basics.FiscalPeriod.Queries
{
    public static class GetFiscalYearPeriodModules
    {
        public record Query(int FiscalYearPeriodId) : IRequest<List<FiscalYearPeriodModuleDto>>;

        public class Handler : IRequestHandler<Query, List<FiscalYearPeriodModuleDto>>
        {
            private readonly IFiscalYearPeriodRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IFiscalYearPeriodRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<List<FiscalYearPeriodModuleDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;
                var modules = await _repo.GetModulesByPeriodIdAsync(request.FiscalYearPeriodId);

                return modules.Select(m => new FiscalYearPeriodModuleDto(
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
                )).ToList();
            }
        }
    }
}