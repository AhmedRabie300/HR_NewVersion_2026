using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using Application.System.MasterData.DocumentTypesGroup.Validators;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Common;
using Application.Abstractions;

namespace Application.System.MasterData.DocumentTypesGroup.Commands
{
    public static class UpdateDocumentTypesGroup
    {
        public record Command(UpdateDocumentTypesGroupDto Data, int Lang = 1) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateDocumentTypesGroupValidator(msg));
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