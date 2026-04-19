using Application.System.MasterData.Abstractions;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;
using Application.Common;

namespace Application.System.MasterData.Document.Commands
{
    public static class DeleteDocument
    {
        public record Command(int Id) : IRequest;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IDocumentRepository _repo;

                        private readonly IValidationMessages _msg;
public Handler(IDocumentRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await _repo.ExistsAsync(request.Id))
                    throw new NotFoundException(_msg.NotFound("Document", request.Id));

                await _repo.DeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

            }
        }
    }
}