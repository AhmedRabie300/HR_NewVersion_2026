using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Domain.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sector.Commands
{
    public static class SoftDeleteSector
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
            private readonly ISectorRepository _repo;

            public Handler(
                ISectorRepository repo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var sector = await _repo.GetByIdAsync(request.Id);
                if (sector == null)
                {
                    throw new NotFoundException(
                        GetMessage("Sector", request.Lang),
                        request.Id,
                        GetFormattedMessage("NotFound", request.Lang, GetMessage("Sector", request.Lang), request.Id)
                    );
                }

                // Check if sector has children
                var children = await _repo.GetByParentIdAsync(request.Id);
                if (children.Any())
                {
                    throw new DomainException(GetFormattedMessage("CannotDeleteHasChildren", request.Lang, GetMessage("Sector", request.Lang)));
                }

                await _repo.SoftDeleteAsync(request.Id, request.RegUserId);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}