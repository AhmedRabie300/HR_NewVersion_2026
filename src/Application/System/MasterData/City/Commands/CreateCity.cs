using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.City.Dtos;
using Application.System.MasterData.City.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.City.Commands
{
    public static class CreateCity
    {
        public record Command(
            int CompanyId,
            int? RegUserId,
            CreateCityDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                var lang = contextService.GetCurrentLanguage();

              
                RuleFor(x => x.Data)
                    .SetValidator(new CreateCityValidator(localizer, contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ICityRepository _repo;
            private readonly ICountryRepository _countryRepo;
            private readonly ICodeGenerationService _codeGenerationService;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ICityRepository repo,
                ICountryRepository countryRepo,
                ICodeGenerationService codeGenerationService,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _countryRepo = countryRepo;
                _codeGenerationService = codeGenerationService;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();
 
                var code = await _codeGenerationService.GenerateCodeAsync(
                    request.CompanyId,
                    request.Data.Code,
                    _repo.GetMaxCodeAsync,
                    (code, ct) => _repo.CodeExistsAsync(code),
                    cancellationToken
                );

                var entity = new Domain.System.MasterData.City(
                    code: code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    phoneKey: request.Data.PhoneKey,
                    regionId: request.Data.RegionId,
                    timeZone: request.Data.TimeZone,
                    countryId: request.Data.CountryId,
                    remarks: request.Data.Remarks,
                    regUserId: request.RegUserId,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}