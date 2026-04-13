using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Groups.Commands
{
    public static class DeleteGroup
    {
        public record Command(int Id) : IRequest<bool>;

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
                    .GreaterThan(0)
                    .WithMessage(_localizer.GetMessage("IdGreaterThanZero", lang));
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IGroupRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(IGroupRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var exists = await _repo.ExistsAsync(request.Id);
                if (!exists)
                    return false;

                var hasUsers = await _repo.HasUsersAsync(request.Id);
                if (hasUsers)
                    throw new ConflictException(
                        _localizer.GetMessage("Group", lang),
                        "Id",
                        request.Id.ToString(),
                        string.Format(_localizer.GetMessage("CannotDeleteHasChildren", lang), _localizer.GetMessage("Group", lang)));

                await _repo.DeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}