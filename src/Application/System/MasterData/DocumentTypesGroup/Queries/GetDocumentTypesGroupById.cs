using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DocumentTypesGroup.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.DocumentTypesGroup.Queries
{
    public static class GetDocumentTypesGroupById
    {
        public record Query(int Id) : IRequest<DocumentTypesGroupDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly IContextService _contextService;

            public Validator(ILocalizationService localizer, IContextService contextService)
            {
                _localizer = localizer;
                _contextService = contextService;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _contextService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, DocumentTypesGroupDto>
        {
            private readonly IDocumentTypesGroupRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(IDocumentTypesGroupRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<DocumentTypesGroupDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("DocumentTypesGroup", lang),
                        request.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("DocumentTypesGroup", lang), request.Id));

                return new DocumentTypesGroupDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}