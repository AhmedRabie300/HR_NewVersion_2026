using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationAndEndOfService.VacationsType.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationAndEndOfService.VacationsType.Queries
{
    public static class GetVacationsTypeById
    {
        public record Query(int Id) : IRequest<VacationsTypeDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {

            }
        }

        public class Handler : IRequestHandler<Query, VacationsTypeDto>
        {
            private readonly IVacationsTypeRepository _repo;

            private readonly IValidationMessages _msg;
            public Handler(IVacationsTypeRepository repo, IContextService contextService, ILocalizationService localizer, IValidationMessages msg)
            {
                _repo = repo;
   
                _msg = msg;
            }

            public async Task<VacationsTypeDto> Handle(Query request, CancellationToken cancellationToken)
            {

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("VacationsType", request.Id));


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