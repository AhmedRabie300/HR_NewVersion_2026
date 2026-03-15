// Application/UARbac/Menus/Commands/UpdateMenu.cs
using Application.UARbac.Abstractions;
using Application.UARbac.Menus.Dtos;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Menus.Commands
{
    public static class UpdateMenu
    {
        public record Command(UpdateMenuDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data.Id)
                    .GreaterThan(0)
                    .WithMessage("Menu ID is required");

                RuleFor(x => x.Data.EngName)
                    .MaximumLength(200);

                RuleFor(x => x.Data.ArbName)
                    .MaximumLength(200);

                RuleFor(x => x.Data.Shortcut)
                    .MaximumLength(50);

                RuleFor(x => x.Data.Image)
                    .MaximumLength(500);

                RuleFor(x => x.Data)
                    .Must(x => x.EngName != null ||
                               x.ArbName != null ||
                               x.ArbName4S != null ||
                               x.Shortcut != null ||
                               x.Rank != null ||
                               x.ParentId != null ||
                               x.FormId != null ||
                               x.ObjectId != null ||
                               x.ViewFormId != null ||
                               x.IsHide != null ||
                               x.Image != null ||
                               x.ViewType != null)
                    .WithMessage("At least one field must be provided to update");
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IMenuRepository _repo;

            public Handler(IMenuRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var menu = await _repo.GetByIdAsync(request.Data.Id);
                if (menu == null)
                    throw new Exception($"Menu with ID {request.Data.Id} not found");

                // Check if parent exists and not self
                if (request.Data.ParentId.HasValue)
                {
                    if (request.Data.ParentId == menu.Id)
                        throw new Exception("Menu cannot be parent of itself");

                    var parentExists = await _repo.ExistsAsync(request.Data.ParentId.Value);
                    if (!parentExists)
                        throw new Exception($"Parent menu with ID {request.Data.ParentId} not found");
                }

                // Update basic info
                menu.UpdateBasicInfo(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Shortcut,
                    request.Data.Rank,
                    request.Data.Image,
                    request.Data.ViewType
                );

                // Update relations
                menu.UpdateRelations(
                    request.Data.ParentId,
                    request.Data.FormId,
                    request.Data.ObjectId,
                    request.Data.ViewFormId
                );

                // Update visibility
                menu.UpdateVisibility(request.Data.IsHide);

                await _repo.UpdateAsync(menu);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}   