using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Document.Dtos;
using Application.System.MasterData.Document.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Document.Commands
{
    public static class CreateDocument
    {
        public record Command(
            int CompanyId,
            int? RegUserId,
            CreateDocumentDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                var lang = contextService.GetCurrentLanguage();
 
                RuleFor(x => x.Data)
                    .SetValidator(new CreateDocumentValidator(localizer, contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IDocumentRepository _repo;
            private readonly IDocumentTypesGroupRepository _docTypeGroupRepo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ICodeGenerationService _codeGenerationService;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IDocumentRepository repo,
                IDocumentTypesGroupRepository docTypeGroupRepo,
                ICompanyRepository companyRepo,
                ICodeGenerationService codeGenerationService,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _docTypeGroupRepo = docTypeGroupRepo;
                _companyRepo = companyRepo;
                _codeGenerationService = codeGenerationService;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(request.CompanyId);
             
                if (request.Data.DocumentTypesGroupId.HasValue)
                {
                    var docTypeGroup = await _docTypeGroupRepo.GetByIdAsync(request.Data.DocumentTypesGroupId.Value);
                    if (docTypeGroup == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("DocumentTypesGroup", lang),
                            request.Data.DocumentTypesGroupId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("DocumentTypesGroup", lang), request.Data.DocumentTypesGroupId.Value));
                }

                var code = await _codeGenerationService.GenerateCodeAsync(
                    request.CompanyId,
                    request.Data.Code,
                    (companyId, ct) => _repo.GetMaxCodeAsync(companyId, ct),
                    (code, ct) => _repo.CodeExistsAsync(code),
                    cancellationToken
                );

                var entity = new Domain.System.MasterData.Document(
                    code: code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    isForCompany: request.Data.IsForCompany,
                    remarks: request.Data.Remarks,
                    regUserId: request.RegUserId,
                    regComputerId: request.Data.RegComputerId,
                    documentTypesGroupId: request.Data.DocumentTypesGroupId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}