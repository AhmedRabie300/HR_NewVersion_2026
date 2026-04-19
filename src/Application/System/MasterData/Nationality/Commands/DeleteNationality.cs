using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Nationality.Commands
{
    public static class DeleteNationality
    {
        public record Command(int Id) : IRequest;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly INationalityRepository _repo;

                        private readonly IValidationMessages _msg;
public Handler(INationalityRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await _repo.ExistsAsync(request.Id))
                    throw new NotFoundException(_msg.NotFound("Nationality", request.Id));

                await _repo.DeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

            }
        }
    }
}