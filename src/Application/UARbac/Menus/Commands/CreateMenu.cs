using Application.UARbac.Abstractions;
using Domain.System;
using Domain.UARbac;
using FluentValidation;
using MediatR;
using Application.UARbac.Menus.Dtos;
using Application.Common;

namespace Application.UARbac.Menus.Commands
{
    public static class CreateMenu
    {
        public record Command(CreateMenuDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data.Code)
                    .MaximumLength(50);

                RuleFor(x => x.Data.EngName)
                    .MaximumLength(200);

                RuleFor(x => x.Data.ArbName)
                    .MaximumLength(200);

                RuleFor(x => x.Data.Shortcut)
                    .MaximumLength(50);

                RuleFor(x => x.Data.Image)
                    .MaximumLength(500);

            
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IMenuRepository _repo;

            public Handler(IMenuRepository repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                // Check if code exists (if provided)
                if (!string.IsNullOrWhiteSpace(request.Data.Code))
                {
                    var existing = await _repo.GetByCodeAsync(request.Data.Code);
                    if (existing != null)
                        throw new ConflictException($"Menu with code '{request.Data.Code}' already exists");
                }

                // Check if parent exists
                if (request.Data.ParentId.HasValue)
                {
                    var parentExists = await _repo.ExistsAsync(request.Data.ParentId.Value);
                    if (!parentExists)
                        throw new NotFoundException("Create Menu", $"Parent menu with ID {request.Data.ParentId} not found");
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