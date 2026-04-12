using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using Application.System.MasterData.Sponsor.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Sponsor.Commands
{
    public static class UpdateSponsor
    {
        public record Command(UpdateSponsorDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                _ContextService = ContextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new UpdateSponsorValidator(_localizer, _ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ISponsorRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ISponsorRepository repo,
                ICompanyRepository companyRepo,
                IContextService ContextService,
                ILocalizationService localizer
              )
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _ContextService = ContextService;
                _localizer = localizer;
            }

     

        

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Sponsor", lang),
                        request.Data.Id));

                 if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Sponsor does not belong to your company");

                // ✅ Update حسب الـ Entity
                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.SponsorNumber
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}