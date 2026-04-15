using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Bank.Dtos;
using Application.System.MasterData.Bank.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Bank.Commands
{
    public static class CreateBank
    {
        public record Command(
            int CompanyId,
            int? RegUserId,
            CreateBankDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                var lang = contextService.GetCurrentLanguage();

               
                RuleFor(x => x.Data)
                    .SetValidator(new CreateBankValidator(localizer, contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IBankRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ICodeGenerationService _codeGenerationService;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IBankRepository repo,
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

                var entity = new Domain.System.MasterData.Bank(
                    code: code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    companyId: request.CompanyId,
                    remarks: request.Data.Remarks,
                    regUserId: request.RegUserId,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}