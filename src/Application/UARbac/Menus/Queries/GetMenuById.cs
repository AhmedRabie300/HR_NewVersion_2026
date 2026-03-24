// Application/UARbac/Menus/Queries/GetMenuById.cs
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
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Query, MenuDetailsDto>
        {
            private readonly IMenuRepository _repo;

            public Handler(IMenuRepository repo)
            {
                _repo = repo;
            }

            public async Task<MenuDetailsDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var menu = await _repo.GetByIdAsync(request.Id);
                if (menu == null)
                    throw new Exception($"Menu with ID {request.Id} not found");

                return new MenuDetailsDto(
                    menu.Id,
                    menu.Code,
                    menu.EngName,
                    menu.ArbName,
                    menu.ArbName4S,
                    menu.ParentId,
                    menu.Parent?.EngName ?? menu.Parent?.ArbName,  
                    menu.Shortcut,
                    menu.Rank,
                    menu.FormId,
                    menu.ObjectId,
                    menu.ViewFormId,
                    menu.IsHide,
                    menu.Image,
                    menu.ViewType,
                    menu.RegUserId,
                    menu.regComputerId,
                    menu.RegDate,
                    menu.CancelDate,
                    menu.Children?.Count ?? 0
                );
            }
        }
    }
}