using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.HICompanies.Commands
{
    public static class DeleteHICompanyClass
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
            private readonly IHICompanyRepository _repo;

            public Handler(IHICompanyRepository repo)
            {
                _repo = repo;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await _repo.ClassExistsAsync(request.Id))
                    return false;

                await _repo.DeleteClassAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}