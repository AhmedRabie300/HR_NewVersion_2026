using MediatR;
using Application.UARbac.Abstractions;
using Domain.UARbac;
using Application.UARbac.Menus.Dtos;

namespace Application.UARbac.Menus.Queries
{
    public static class GetMenuTree
    {
        public record Query(bool IncludeHidden = false) : IRequest<List<MenuTreeDto>>;

        public class Handler : IRequestHandler<Query, List<MenuTreeDto>>
        {
            private readonly IMenuRepository _repo;

            public Handler(IMenuRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<MenuTreeDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var allMenus = await _repo.GetAllAsync();
                    var rootMenus = request.IncludeHidden
                      ? allMenus.Where(x => x.ParentId == null).ToList()
    : allMenus.Where(x => x.ParentId == null && (x.IsHide == null || x.IsHide == false)).ToList();


                    var result = BuildTree(rootMenus, allMenus);

                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR in GetMenuTree.Handler: {ex.Message}");
                    Console.WriteLine($"Stack: {ex.StackTrace}");
                    throw;
                }
            }

            private List<MenuTreeDto> BuildTree(List<Menu> rootMenus, List<Menu> allMenus)
            {
                if (rootMenus == null || !rootMenus.Any())
                    return new List<MenuTreeDto>();

                return rootMenus
                    .Where(m => m != null)
                    .Select(m => new MenuTreeDto(
                        m.Id,
                        m.Code,
                        m.EngName ?? m.ArbName ?? "",
                        m.ParentId,
                        m.Rank,
                        m.Image,
                        m.FormId.HasValue ? $"/form/{m.FormId}" : null,
                        GetChildren(m.Id, allMenus)
                    ))
                    .OrderBy(x => x.Rank)
                    .ToList();
            }

            private List<MenuTreeDto> GetChildren(int parentId, List<Menu> allMenus)
            {
                var children = allMenus.Where(x => x.ParentId == parentId).ToList();
                if (!children.Any())
                    return null;

                return children.Select(c => new MenuTreeDto(
                    c.Id,
                    c.Code,
                    c.EngName ?? c.ArbName ?? "",
                    c.ParentId,
                    c.Rank,
                    c.Image,
                    c.FormId.HasValue ? $"/form/{c.FormId}" : null,
                    GetChildren(c.Id, allMenus)
                )).OrderBy(x => x.Rank).ToList();
            }
        }
    }
}