using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Project.Commands
{
    public static class SoftDeleteProject
    {
        public record Command(int Id, int? RegUserId = null) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                var lang = _contextService.GetCurrentLanguage();
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(_localizer.GetMessage("IdGreaterThanZero", lang));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IProjectRepository _repo;

            public Handler(IProjectRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _repo.SoftDeleteAsync(request.Id, request.RegUserId);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}