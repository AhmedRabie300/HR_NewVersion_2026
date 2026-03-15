// Application/UARbac/Menus/Commands/DeleteMenu.cs
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Menus.Commands
{
    public static class DeleteMenu
    {
        public record Command(int Id) : IRequest<bool>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0)
                    .WithMessage("Menu ID must be greater than 0");
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IMenuRepository _repo;

            public Handler(IMenuRepository repo)
            {
                _repo = repo;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var exists = await _repo.ExistsAsync(request.Id);
                if (!exists)
                    return false;

                await _repo.DeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}