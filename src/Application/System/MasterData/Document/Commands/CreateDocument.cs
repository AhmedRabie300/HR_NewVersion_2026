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
        public record Command(CreateDocumentDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,IDocumentRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateDocumentValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IDocumentRepository _repo;
            private readonly IValidationMessages _msg;
            private readonly IDocumentTypesGroupRepository _docTypeGroupRepo;
            public Handler(
                IDocumentRepository repo, IValidationMessages msg,
                IDocumentTypesGroupRepository docTypeGroupRepo)
            {
                _repo = repo;
                _msg = msg;
                _docTypeGroupRepo = docTypeGroupRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                                var codeExists = await _repo.CodeExistsAsync(request.Data.Code);
                if (codeExists)
                {
                    throw new ConflictException(_msg.CodeExists("Document", request.Data.Code));
                }

                if (request.Data.DocumentTypesGroupId.HasValue)
                {
                    var docTypeGroup = await _docTypeGroupRepo.GetByIdAsync(request.Data.DocumentTypesGroupId.Value);
                    if (docTypeGroup == null)
                        throw new NotFoundException(_msg.NotFound("DocumentTypesGroup", request.Data.DocumentTypesGroupId.Value));
                }

                var entity = new Domain.System.MasterData.Document(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    isForCompany: request.Data.IsForCompany,
                    remarks: request.Data.Remarks,
                    documentTypesGroupId: request.Data.DocumentTypesGroupId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}