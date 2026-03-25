using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Document.Dtos;
using Application.System.MasterData.Document.Validators;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Document.Commands
{
    public static class UpdateDocument
    {
        public record Command(UpdateDocumentDto Data, int Lang = 1) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .Custom((data, context) =>
                    {
                        var lang = context.InstanceToValidate.Lang;
                        var validator = new UpdateDocumentValidator(localizer, lang);
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

        public class Handler : IRequestHandler<Command, Unit>
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

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", request.Lang),
                        _localizer.GetMessage("Document", request.Lang),
                        request.Data.Id));

                if (request.Data.DocumentTypesGroupId.HasValue)
                {
                    var docTypeGroup = await _docTypeGroupRepo.GetByIdAsync(request.Data.DocumentTypesGroupId.Value);
                    if (docTypeGroup == null)
                        throw new Exception(string.Format(
                            _localizer.GetMessage("NotFound", request.Lang),
                            _localizer.GetMessage("DocumentTypesGroup", request.Lang),
                            request.Data.DocumentTypesGroupId));
                }

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.IsForCompany,
                    request.Data.Remarks,
                    request.Data.DocumentTypesGroupId
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}