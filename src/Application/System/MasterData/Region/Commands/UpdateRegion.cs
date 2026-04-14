using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Region.Dtos;
using Application.System.MasterData.Region.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Region.Commands
{
    public static class UpdateRegion
    {
        public record Command(UpdateRegionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new UpdateRegionValidator(_localizer, _contextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IRegionRepository _repo;
            private readonly ICountryRepository _countryRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IRegionRepository repo,
                ICountryRepository countryRepo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _countryRepo = countryRepo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Region", lang),
                        request.Data.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Region", lang), request.Data.Id));

                if (request.Data.CountryId.HasValue)
                {
                    var country = await _countryRepo.GetByIdAsync(request.Data.CountryId.Value);
                    if (country == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("Country", lang),
                            request.Data.CountryId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Country", lang), request.Data.CountryId.Value));
                }

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.CountryId,
                    request.Data.CompanyId,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}