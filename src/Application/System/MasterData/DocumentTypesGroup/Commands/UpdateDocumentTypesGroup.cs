using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using Application.System.MasterData.DocumentTypesGroup.Validators;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Common;

namespace Application.System.MasterData.DocumentTypesGroup.Commands
{
    public static class UpdateDocumentTypesGroup
    {
        public record Command(UpdateDocumentTypesGroupDto Data, int Lang = 1) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateDocumentTypesGroupValidator(localizer, ContextService));
            }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDocumentTypesGroupRepository _repo;
            private readonly ILocalizationService _localizer;

            public Handler(IDocumentTypesGroupRepository repo, ILocalizationService localizer)
            {
                _repo = repo;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException("NotFound",string.Format(
                        _localizer.GetMessage("NotFound", request.Lang),
                        _localizer.GetMessage("DocumentTypesGroup", request.Lang),
                        request.Data.Id));

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