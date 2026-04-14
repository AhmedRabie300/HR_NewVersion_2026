using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationsType.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationsType.Queries
{
    public static class GetVacationsTypeById
    {
        public record Query(int Id) : IRequest<VacationsTypeDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly IContextService _contextService;

            public Validator(ILocalizationService localizer, IContextService contextService)
            {
                _localizer = localizer;
                _contextService = contextService;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _contextService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, VacationsTypeDto>
        {
            private readonly IVacationsTypeRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(IVacationsTypeRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<VacationsTypeDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("VacationsType", lang),
                        request.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("VacationsType", lang), request.Id));

                return new VacationsTypeDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    IsPaid: entity.IsPaid,
                    Sex: entity.Sex,
                    IsAnnual: entity.IsAnnual,
                    IsSickVacation: entity.IsSickVacation,
                    IsFromAnnual: entity.IsFromAnnual,
                    ForSalaryTransaction: entity.ForSalaryTransaction,
                    CompanyId: entity.CompanyId,
                    CompanyName: entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    OBalanceTransactionId: entity.OBalanceTransactionId,
                    OverDueVacationId: entity.OverDueVacationId,
                    Stage1Pct: entity.Stage1Pct,
                    Stage2Pct: entity.Stage2Pct,
                    Stage3Pct: entity.Stage3Pct,
                    ForDeductionTransaction: entity.ForDeductionTransaction,
                    AffectEos: entity.AffectEos,
                    VactionTypeCaculation: entity.VactionTypeCaculation,
                    ExceededDaysType: entity.ExceededDaysType,
                    HasPayment: entity.HasPayment,
                    RoundAnnualVacBalance: entity.RoundAnnualVacBalance,
                    Religion: entity.Religion,
                    IsOfficial: entity.IsOfficial,
                    OverlapWithAnotherVac: entity.OverlapWithAnotherVac,
                    ConsiderAllowedDays: entity.ConsiderAllowedDays,
                    TimesNoInYear: entity.TimesNoInYear,
                    AllowedDaysNo: entity.AllowedDaysNo,
                    ExcludedFromSsRequests: entity.ExcludedFromSsRequests,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}