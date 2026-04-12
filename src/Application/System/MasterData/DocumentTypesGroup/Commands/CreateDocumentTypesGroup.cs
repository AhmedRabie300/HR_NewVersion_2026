using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using Application.System.MasterData.DocumentTypesGroup.Validators;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Common;

namespace Application.System.MasterData.DocumentTypesGroup.Commands
{
    public static class CreateDocumentTypesGroup
    {
        public record Command(CreateDocumentTypesGroupDto Data, int Lang = 1) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateDocumentTypesGroupValidator(localizer, ContextService));
            }
        }
        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IDocumentTypesGroupRepository _repo;
            private readonly ILocalizationService _localizer;

            public Handler(IDocumentTypesGroupRepository repo, ILocalizationService localizer)
            {
                _repo = repo;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", request.Lang),
                        _localizer.GetMessage("DocumentTypesGroup", request.Lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.DocumentTypesGroup(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.RegComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}