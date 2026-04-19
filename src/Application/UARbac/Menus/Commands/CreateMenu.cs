using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Menus.Dtos;
using Application.UARbac.Menus.Validators;
using Domain.UARbac;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Menus.Commands
{
    public static class CreateMenu
    {
        public record Command(CreateMenuDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateMenuValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IMenuRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IMenuRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!string.IsNullOrWhiteSpace(request.Data.Code))
                {
                    var existing = await _repo.GetByCodeAsync(request.Data.Code);
                    if (existing != null)
                        throw new ConflictException(_msg.CodeExists("Menu", request.Data.Code));
                }

                if (request.Data.ParentId.HasValue)
                {
                    var parentExists = await _repo.ExistsAsync(request.Data.ParentId.Value);
                    if (!parentExists)
                        throw new NotFoundException(_msg.NotFound("ParentMenu", request.Data.ParentId.Value));
                }

                var menu = new Menu(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.ParentId,
                    request.Data.Shortcut,
                    request.Data.Rank,
                    request.Data.FormId,
                    request.Data.ObjectId,
                    request.Data.ViewFormId,
                    request.Data.IsHide,
                    request.Data.Image,
                    request.Data.ViewType,
                    request.Data.RegUserId,
                    request.Data.regComputerId
                );

                await _repo.AddAsync(menu);
                await _repo.SaveChangesAsync(cancellationToken);

                return menu.Id;
            }
        }
    }
}
