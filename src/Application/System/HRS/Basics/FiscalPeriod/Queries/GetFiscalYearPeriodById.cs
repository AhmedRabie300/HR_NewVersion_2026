using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.FiscalPeriod.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.FiscalPeriod.Queries
{
    public static class GetFiscalYearPeriodById
    {
        public record Query(int Id) : IRequest<FiscalYearPeriodDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ICurrentUser _currentUser;

            public Validator(ICurrentUser currentUser)
            {
                _currentUser = currentUser;
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _currentUser.Language == 2
                        ? "المعرف يجب أن يكون أكبر من 0"
                        : "ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Query, FiscalYearPeriodDto>
        {
            private readonly IFiscalYearPeriodRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IFiscalYearPeriodRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<FiscalYearPeriodDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _currentUser.Language;

                var entity = await _repo.GetByIdAsync(request.Id);
          

                return new FiscalYearPeriodDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    FiscalYearId: entity.FiscalYearId,
                    FiscalYearName: lang == 2 ? entity.FiscalYear?.ArbName : entity.FiscalYear?.EngName,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    FromDate: entity.FromDate,
                    ToDate: entity.ToDate,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    HFromDate: entity.HFromDate,
                    HToDate: entity.HToDate,
                    PeriodType: entity.PeriodType,
                    PeriodRank: entity.PeriodRank,
                    PrepareFromDate: entity.PrepareFromDate,
                    PrepareToDate: entity.PrepareToDate,
                    CompanyId: entity.CompanyId,
                    CompanyName: lang == 2 ? entity.Company?.ArbName : entity.Company?.EngName,
                    IsActive: entity.IsActive(),
                    Modules: entity.Modules.Select(m => new FiscalYearPeriodModuleDto(
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
                );
            }
        }
    }
}