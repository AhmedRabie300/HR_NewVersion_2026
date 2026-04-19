using Application.System.MasterData.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;
using Application.Common.Abstractions;
using Application.Common;

namespace Application.System.MasterData.Religion.Commands
{
    public static class DeleteReligion
    {
        public record Command(int Id) : IRequest;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IReligionRepository _repo;

                        private readonly IValidationMessages _msg;
public Handler(IReligionRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await _repo.ExistsAsync(request.Id))
                    throw new NotFoundException(_msg.NotFound("Religion", request.Id));

                await _repo.DeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

            }
        }
    }
}