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
        public record Command(CreateDocumentDto Data, int Lang = 1) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .Custom((data, context) =>
                    {
                        var lang = context.InstanceToValidate.Lang;
                        var validator = new CreateDocumentValidator(localizer, lang);
                        var result = validator.Validate(data);
                        if (!result.IsValid)
                        {
                            foreach (var error in result.Errors)
                            {
                                context.AddFailure(error);
                            }
                        }
                    });
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IDocumentRepository _repo;
            private readonly IDocumentTypesGroupRepository _docTypeGroupRepo;
            private readonly ILocalizationService _localizer;

            public Handler(IDocumentRepository repo, IDocumentTypesGroupRepository docTypeGroupRepo, ILocalizationService localizer)
            {
                _repo = repo;
                _docTypeGroupRepo = docTypeGroupRepo;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.Data.DocumentTypesGroupId.HasValue)
                {
                    var docTypeGroup = await _docTypeGroupRepo.GetByIdAsync(request.Data.DocumentTypesGroupId.Value);
                    if (docTypeGroup == null)
                        throw new NotFoundException("Create Document", string.Format(
                            _localizer.GetMessage("NotFound", request.Lang),
                            _localizer.GetMessage("DocumentTypesGroup", request.Lang),
                            request.Data.DocumentTypesGroupId));
                }

                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", request.Lang),
                        _localizer.GetMessage("Document", request.Lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.Document(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.IsForCompany,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.RegComputerId,
                    request.Data.DocumentTypesGroupId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}