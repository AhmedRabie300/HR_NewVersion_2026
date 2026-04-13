using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Document.Dtos;
using Application.System.MasterData.Document.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Document.Commands
{
    public static class CreateDocument
    {
        public record Command(CreateDocumentDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new CreateDocumentValidator(_localizer, _contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IDocumentRepository _repo;
            private readonly IDocumentTypesGroupRepository _docTypeGroupRepo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IDocumentRepository repo,
                IDocumentTypesGroupRepository docTypeGroupRepo,
                ICompanyRepository companyRepo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _docTypeGroupRepo = docTypeGroupRepo;
                _companyRepo = companyRepo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _contextService.GetCurrentCompanyId();
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(companyId);
                if (company == null)
                    throw new NotFoundException("Create Document", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        companyId));

                if (request.Data.DocumentTypesGroupId.HasValue)
                {
                    var docTypeGroup = await _docTypeGroupRepo.GetByIdAsync(request.Data.DocumentTypesGroupId.Value);
                    if (docTypeGroup == null)
                        throw new NotFoundException("Create Document", string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("DocumentTypesGroup", lang),
                            request.Data.DocumentTypesGroupId));
                }

                string code;

                if (company.HasSequence == true)
                {
                    var prefix = company.Prefix;
                    var separator = company.Separator ?? "-";
                    var sequenceLength = company.SequenceLength ?? 5;

                    var maxCode = await _repo.GetMaxCodeAsync(companyId, cancellationToken);
                    int lastNumber = 0;

                    if (!string.IsNullOrEmpty(maxCode))
                    {
                        if (maxCode.Contains(separator))
                        {
                            var lastPart = maxCode.Split(separator).Last();
                            int.TryParse(lastPart, out lastNumber);
                        }
                        else
                        {
                            int.TryParse(maxCode, out lastNumber);
                        }
                    }

                    var newNumber = lastNumber + 1;
                    var formattedNumber = newNumber.ToString($"D{sequenceLength}");
                    code = $"{prefix}{separator}{formattedNumber}";
                }
                else
                {
                    code = request.Data.Code;

                    if (string.IsNullOrWhiteSpace(code))
                        throw new Exception(_localizer.GetMessage("CodeRequired", lang));

                    if (await _repo.CodeExistsAsync(code))
                        throw new ConflictException(string.Format(
                            _localizer.GetMessage("CodeExists", lang),
                            _localizer.GetMessage("Document", lang),
                            code));
                }

                var entity = new Domain.System.MasterData.Document(
                    code: code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    isForCompany: request.Data.IsForCompany,
                    remarks: request.Data.Remarks,
                    regUserId: request.Data.RegUserId,
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