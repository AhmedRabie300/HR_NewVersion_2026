using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Religion.Dtos;
using Application.System.MasterData.Religion.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Religion.Commands
{
    public static class CreateReligion
    {
        public record Command(
            int CompanyId,
            int? RegUserId,
            CreateReligionDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                var lang = contextService.GetCurrentLanguage();

               
                RuleFor(x => x.Data)
                    .SetValidator(new CreateReligionValidator(localizer, contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IReligionRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ICodeGenerationService _codeGenerationService;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IReligionRepository repo,
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

                var entity = new Domain.System.MasterData.Religion(
                    code: code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    remarks: request.Data.Remarks,
                    regUserId: request.RegUserId,
                    regComputerId: request.Data.regComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}