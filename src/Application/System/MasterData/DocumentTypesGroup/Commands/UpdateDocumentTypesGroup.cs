using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using Application.System.MasterData.DocumentTypesGroup.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.DocumentTypesGroup.Commands
{
    public static class UpdateDocumentTypesGroup
    {
        public record Command(UpdateDocumentTypesGroupDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IDocumentTypesGroupRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateDocumentTypesGroupValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDocumentTypesGroupRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IDocumentTypesGroupRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("DocumentTypesGroup", request.Data.Id));

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}