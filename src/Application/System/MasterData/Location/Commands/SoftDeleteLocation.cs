using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Location.Commands
{
    public static class SoftDeleteLocation
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
            private readonly ILocationRepository _repo;

            public Handler(
                ILocationRepository repo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var location = await _repo.GetByIdAsync(request.Id);
                if (location == null)
                {
                    throw new NotFoundException(
                        GetMessage("Location", request.Lang),
                        request.Id,
                        GetFormattedMessage("NotFound", request.Lang, GetMessage("Location", request.Lang), request.Id)
                    );
                }

                await _repo.SoftDeleteAsync(request.Id, request.RegUserId);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}