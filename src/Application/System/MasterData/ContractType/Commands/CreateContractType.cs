using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.ContractType.Dtos;
using Application.System.MasterData.ContractType.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.ContractType.Commands
{
    public static class CreateContractType
    {
        public record Command(CreateContractTypeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateContractTypeValidator(localizer, languageService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IContractTypeRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(IContractTypeRepository repo, ICompanyRepository companyRepo, ILanguageService languageService, ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _languageService = languageService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _languageService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(request.Data.CompanyId);
                if (company == null)
                    throw new NotFoundException("Create Contract Type", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        request.Data.CompanyId));

                if (await _repo.CodeExistsAsync(request.Data.Code, request.Data.CompanyId))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("ContractType", lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.ContractType(
                    request.Data.Code,
                    request.Data.CompanyId,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.IsSpecial,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.RegComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}