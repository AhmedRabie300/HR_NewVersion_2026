using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Menus.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Menus.Queries
{
    public static class GetMenuById
    {
        public record Query(int Id) : IRequest<MenuDetailsDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _contextService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, MenuDetailsDto>
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

            public async Task<MenuDetailsDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var menu = await _repo.GetByIdAsync(request.Id);
                if (menu == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Menu", lang),
                        request.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Menu", lang), request.Id));

                return new MenuDetailsDto(
                    Id: menu.Id,
                    Code: menu.Code,
                    EngName: menu.EngName,
                    ArbName: menu.ArbName,
                    ArbName4S: menu.ArbName4S,
                    ParentId: menu.ParentId,
                    ParentName: menu.Parent != null ? (lang == 2 ? menu.Parent.ArbName : menu.Parent.EngName) : null,
                    Shortcut: menu.Shortcut,
                    Rank: menu.Rank,
                    FormId: menu.FormId,
                    ObjectId: menu.ObjectId,
                    ViewFormId: menu.ViewFormId,
                    IsHide: menu.IsHide,
                    Image: menu.Image,
                    ViewType: menu.ViewType,
                    RegUserId: menu.RegUserId,
                    regComputerId: menu.regComputerId,
                    RegDate: menu.RegDate,
                    CancelDate: menu.CancelDate,
                    ChildrenCount: menu.Children?.Count ?? 0
                );
            }
        }
    }
}