using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.HRS.VacationsType.Dtos;
using Application.System.HRS.VacationsType.Validators;
using Domain.System.HRS;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationsType.Commands
{
    public static class CreateVacationsType
    {
        public record Command(
            int CompanyId,
            int? RegUserId,
            CreateVacationsTypeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                var lang = contextService.GetCurrentLanguage();
 
                RuleFor(x => x.Data)
                    .SetValidator(new CreateVacationsTypeValidator(localizer, contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IVacationsTypeRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ICodeGenerationService _codeGenerationService;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IVacationsTypeRepository repo,
                ICompanyRepository companyRepo,
                ICodeGenerationService codeGenerationService,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _codeGenerationService = codeGenerationService;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(request.CompanyId);
              
                var code = await _codeGenerationService.GenerateCodeAsync(
                    request.CompanyId,
                    request.Data.Code,
                    (companyId, ct) => _repo.GetMaxCodeAsync(companyId, ct),
                    (code, ct) => _repo.CodeExistsAsync(code),
                    cancellationToken
                );

                var entity = new Domain.System.HRS.VacationsType(
                    code: code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    isPaid: request.Data.IsPaid,
                    sex: request.Data.Sex,
                    isAnnual: request.Data.IsAnnual,
                    isSickVacation: request.Data.IsSickVacation,
                    isFromAnnual: request.Data.IsFromAnnual,
                    forSalaryTransaction: request.Data.ForSalaryTransaction,
                    companyId: request.CompanyId,
                    remarks: request.Data.Remarks,
                    regUserId: request.RegUserId,
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