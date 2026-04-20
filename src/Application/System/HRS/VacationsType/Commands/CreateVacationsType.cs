using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationsType.Dtos;
using Application.System.HRS.VacationsType.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationsType.Commands
{
    public static class CreateVacationsType
    {
        public record Command(
            CreateVacationsTypeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IVacationsTypeRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateVacationsTypeValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IVacationsTypeRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly IValidationMessages _msg;

            public Handler(
                IVacationsTypeRepository repo,
                ICurrentUser currentUser,
                IValidationMessages msg)
            {
                _repo = repo;
                _currentUser = currentUser;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                

                var entity = new Domain.System.HRS.VacationsType(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    isPaid: request.Data.IsPaid,
                    sex: request.Data.Sex,
                    isAnnual: request.Data.IsAnnual,
                    isSickVacation: request.Data.IsSickVacation,
                    isFromAnnual: request.Data.IsFromAnnual,
                    forSalaryTransaction: request.Data.ForSalaryTransaction,
                    remarks: request.Data.Remarks,
                    regComputerId: request.Data.RegComputerId,
                    oBalanceTransactionId: request.Data.OBalanceTransactionId,
                    overDueVacationId: request.Data.OverDueVacationId,
                    stage1Pct: request.Data.Stage1Pct,
                    stage2Pct: request.Data.Stage2Pct,
                    stage3Pct: request.Data.Stage3Pct,
                    forDeductionTransaction: request.Data.ForDeductionTransaction,
                    affectEos: request.Data.AffectEos,
                    vactionTypeCaculation: request.Data.VactionTypeCaculation,
                    exceededDaysType: request.Data.ExceededDaysType,
                    hasPayment: request.Data.HasPayment,
                    roundAnnualVacBalance: request.Data.RoundAnnualVacBalance,
                    religion: request.Data.Religion,
                    isOfficial: request.Data.IsOfficial,
                    overlapWithAnotherVac: request.Data.OverlapWithAnotherVac,
                    considerAllowedDays: request.Data.ConsiderAllowedDays,
                    timesNoInYear: request.Data.TimesNoInYear,
                    allowedDaysNo: request.Data.AllowedDaysNo,
                    excludedFromSsRequests: request.Data.ExcludedFromSsRequests
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}