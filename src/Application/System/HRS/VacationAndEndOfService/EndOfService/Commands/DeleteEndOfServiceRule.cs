using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.VacationAndEndOfService.EndOfService.Commands
{
    public static class DeleteEndOfServiceRule
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
                    .GreaterThan(0).WithMessage(_localizer.GetMessage("IdGreaterThanZero", lang));
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IEndOfServiceRepository _repo;

            public Handler(IEndOfServiceRepository repo)
            {
                _repo = repo;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await _repo.RuleExistsAsync(request.Id))
                    return false;

                await _repo.DeleteRuleAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}