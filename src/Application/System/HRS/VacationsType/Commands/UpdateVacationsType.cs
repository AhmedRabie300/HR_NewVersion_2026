using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.VacationsType.Dtos;
using Application.System.HRS.VacationsType.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationsType.Commands
{
    public static class UpdateVacationsType
    {
        public record Command(UpdateVacationsTypeDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new UpdateVacationsTypeValidator(_localizer, _contextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IVacationsTypeRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;
            private readonly IValidationMessages _msg;

            public Handler(
                IVacationsTypeRepository repo,
                IContextService contextService,
                ILocalizationService localizer,
                IValidationMessages msg
                )
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("VacationsType", request.Data.Id));
  

                 if (request.Data.Code != null && request.Data.Code != entity.Code)
                {
                    if (await _repo.CodeExistsAsync(request.Data.Code, request.Data.Id))
                        throw new ConflictException(string.Format(
                            _localizer.GetMessage("CodeExists", lang),
                            _localizer.GetMessage("VacationsType", lang),
                            request.Data.Code));
                }

                entity.Update(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.IsPaid,
                    request.Data.Sex,
                    request.Data.IsAnnual,
                    request.Data.IsSickVacation,
                    request.Data.IsFromAnnual,
                    request.Data.ForSalaryTransaction,
                    request.Data.Remarks,
                    request.Data.OBalanceTransactionId,
                    request.Data.OverDueVacationId,
                    request.Data.Stage1Pct,
                    request.Data.Stage2Pct,
                    request.Data.Stage3Pct,
                    request.Data.ForDeductionTransaction,
                    request.Data.AffectEos,
                    request.Data.VactionTypeCaculation,
                    request.Data.ExceededDaysType,
                    request.Data.HasPayment,
                    request.Data.RoundAnnualVacBalance,
                    request.Data.Religion,
                    request.Data.IsOfficial,
                    request.Data.OverlapWithAnotherVac,
                    request.Data.ConsiderAllowedDays,
                    request.Data.TimesNoInYear,
                    request.Data.AllowedDaysNo,
                    request.Data.ExcludedFromSsRequests
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}