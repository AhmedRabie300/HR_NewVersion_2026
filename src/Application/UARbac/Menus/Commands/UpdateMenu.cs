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
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateMenuValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IMenuRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IMenuRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var menu = await _repo.GetByIdAsync(request.Data.Id);
                if (menu == null)
                    throw new NotFoundException(_msg.NotFound("Menu", request.Data.Id));

                if (request.Data.ParentId.HasValue)
                {
                    if (request.Data.ParentId == menu.Id)
                        throw new ConflictException(_msg.Get("MenuCannotBeParentOfItself"));

                    var parentExists = await _repo.ExistsAsync(request.Data.ParentId.Value);
                    if (!parentExists)
                        throw new NotFoundException(_msg.NotFound("ParentMenu", request.Data.ParentId.Value));
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
