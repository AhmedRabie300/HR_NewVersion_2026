using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Menus.Dtos;
using Application.UARbac.Menus.Validators;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Menus.Commands
{
    public static class UpdateMenu
    {
        public record Command(UpdateMenuDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new UpdateMenuValidator(_contextService, _localizer));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IMenuRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(IMenuRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var menu = await _repo.GetByIdAsync(request.Data.Id);
                if (menu == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Menu", lang),
                        request.Data.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Menu", lang), request.Data.Id));

                if (request.Data.ParentId.HasValue)
                {
                    if (request.Data.ParentId == menu.Id)
                        throw new ConflictException(
                            _localizer.GetMessage("Menu", lang),
                            "ParentId",
                            request.Data.ParentId.Value.ToString(),
                            _localizer.GetMessage("MenuCannotBeParentOfItself", lang));

                    var parentExists = await _repo.ExistsAsync(request.Data.ParentId.Value);
                    if (!parentExists)
                        throw new NotFoundException(
                            _localizer.GetMessage("ParentMenu", lang),
                            request.Data.ParentId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("ParentMenu", lang), request.Data.ParentId.Value));
                }

                menu.UpdateBasicInfo(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Shortcut,
                    request.Data.Rank,
                    request.Data.Image,
                    request.Data.ViewType
                );

                menu.UpdateRelations(
                    request.Data.ParentId,
                    request.Data.FormId,
                    request.Data.ObjectId,
                    request.Data.ViewFormId
                );

                menu.UpdateVisibility(request.Data.IsHide);

                await _repo.UpdateAsync(menu);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}