using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Document.Dtos;
using Application.System.MasterData.Document.Validators;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Common;
using Application.Abstractions;

namespace Application.System.MasterData.Document.Commands
{
    public static class UpdateDocument
    {
        public record Command(UpdateDocumentDto Data, int Lang = 1) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateDocumentValidator(msg));
            }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDocumentRepository _repo;
                        private readonly IValidationMessages _msg;
private readonly IDocumentTypesGroupRepository _docTypeGroupRepo;
            public Handler(IDocumentRepository repo, IValidationMessages msg, IDocumentTypesGroupRepository docTypeGroupRepo)
            {
                _repo = repo;
                _msg = msg;
                _docTypeGroupRepo = docTypeGroupRepo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Document", request.Data.Id));    
                        

                if (request.Data.DocumentTypesGroupId.HasValue)
                {
                    var docTypeGroup = await _docTypeGroupRepo.GetByIdAsync(request.Data.DocumentTypesGroupId.Value);
                    if (docTypeGroup == null)
                        throw new NotFoundException(_msg.NotFound("DocumentTypesGroup", request.Data.DocumentTypesGroupId.Value));

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