using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Branch.Queries
{
    public static class GetMaxCode
    {
        public record Query : IRequest<string?>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly IContextService _contextService;

            public Validator(ILocalizationService localizer, IContextService contextService)
            {
                _localizer = localizer;
                _contextService = contextService;
            }
        }

        public class Handler : IRequestHandler<Query, string?>
        {
            private readonly IBranchRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IBranchRepository repo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<string?> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _contextService.GetCurrentCompanyId();
                var lang = _contextService.GetCurrentLanguage();

                 var maxCode = await _repo.GetMaxCodeAsync(companyId, cancellationToken);

                return maxCode;
            }
        }
    }
}