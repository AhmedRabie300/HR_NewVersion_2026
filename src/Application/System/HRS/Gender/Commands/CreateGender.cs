using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Gender.Dtos;
using Application.System.HRS.Gender.Validators;
using Application.System.MasterData.Abstractions;
using Domain.System.HRS;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Gender.Commands
{
    public static class CreateGender
    {
        public record Command(
            int CompanyId,
            int? RegUserId,
            CreateGenderDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                var lang = contextService.GetCurrentLanguage();

                
                RuleFor(x => x.Data)
                    .SetValidator(new CreateGenderValidator(localizer, contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IGenderRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ICodeGenerationService _codeGenerationService;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IGenderRepository repo,
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
                    (companyId, ct) => _repo.GetMaxCodeAsync(ct),
                    (code, ct) => _repo.CodeExistsAsync(code),
                    cancellationToken
                );

                var entity = new Domain.System.HRS.Gender(
                    code: code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    regUserId: request.RegUserId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}