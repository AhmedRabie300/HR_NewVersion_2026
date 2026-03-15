using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Modules.Commands
{
    public static class DeleteModule
    {
        public record Command(int Id) : IRequest<bool>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage("Valid module ID is required");
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IModuleRepository _repo;

            public Handler(IModuleRepository repo)
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