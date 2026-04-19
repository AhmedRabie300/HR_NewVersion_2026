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
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, MenuDetailsDto>
        {
            private readonly IMenuRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IMenuRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<MenuDetailsDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var menu = await _repo.GetByIdAsync(request.Id);
                if (menu == null)
                    throw new NotFoundException(_msg.NotFound("Menu", request.Id));

                return new MenuDetailsDto(
                    Id: menu.Id,
                    Code: menu.Code,
                    EngName: menu.EngName,
                    ArbName: menu.ArbName,
                    ArbName4S: menu.ArbName4S,
                    ParentId: menu.ParentId,
                    ParentName: menu.Parent?.EngName ?? menu.Parent?.ArbName,
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
