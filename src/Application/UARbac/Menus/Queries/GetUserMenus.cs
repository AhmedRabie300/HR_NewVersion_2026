// Application/UARbac/Menus/Queries/GetUserMenus.cs
using Application.UARbac.Abstractions;
using Application.UARbac.Menus.Dtos;
using Domain.UARbac;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Menus.Queries
{
    public static class GetUserMenus
    {
        public record Query(int UserId, bool VisibleOnly = true) : IRequest<List<MenuTreeDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, List<MenuTreeDto>>
        {
            private readonly IMenuRepository _repo;

            public Handler(IMenuRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<MenuTreeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var menus = request.VisibleOnly
                    ? await _repo.GetUserVisibleMenusAsync(request.UserId)
                    : await _repo.GetUserMenusAsync(request.UserId);

                return BuildTree(menus.Where(x => x.ParentId == null).ToList(), menus.ToList());
            }

            private List<MenuTreeDto> BuildTree(List<Menu> roots, List<Menu> allMenus)
            {
                return roots.Select(m => new MenuTreeDto(
                    m.Id,
                    m.Code,
                    m.EngName ?? m.ArbName ?? "",
                    m.ParentId,
                    m.Rank,
                    m.Image,
                    m.FormId.HasValue ? $"/form/{m.FormId}" : null,
                    BuildTree(allMenus.Where(x => x.ParentId == m.Id).ToList(), allMenus)
                )).OrderBy(x => x.Rank).ToList();
            }
        }
    }
}