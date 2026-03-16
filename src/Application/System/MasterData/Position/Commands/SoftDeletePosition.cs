using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Domain.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Position.Commands
{
    public static class SoftDeletePosition
    {
        public record Command(int Id, int? RegUserId = null, int Lang = 1) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(localization.GetMessage("IdGreaterThanZero", lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Command, Unit>
        {
            private readonly IPositionRepository _repo;

            public Handler(
                IPositionRepository repo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var position = await _repo.GetByIdAsync(request.Id);
                if (position == null)
                {
                    throw new NotFoundException(
                        GetMessage("Position", request.Lang),
                        request.Id,
                        GetFormattedMessage("NotFound", request.Lang, GetMessage("Position", request.Lang), request.Id)
                    );
                }

                // Check if position has children
                var children = await _repo.GetByParentIdAsync(request.Id);
                if (children.Any())
                {
                    throw new DomainException(GetFormattedMessage("CannotDeleteHasChildren", request.Lang, GetMessage("Position", request.Lang)));
                }

                await _repo.SoftDeleteAsync(request.Id, request.RegUserId);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}