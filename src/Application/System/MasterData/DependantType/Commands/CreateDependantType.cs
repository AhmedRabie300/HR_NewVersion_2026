// Application/System/MasterData/DependantType/Commands/CreateDependantType.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DependantType.Dtos;
using Application.System.MasterData.DependantType.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.DependantType.Commands
{
    public static class CreateDependantType
    {
        public record Command(CreateDependantTypeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                _languageService = languageService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new CreateDependantTypeValidator(_localizer, _languageService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IDependantTypeRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IDependantTypeRepository repo,
                ICompanyRepository companyRepo,
                IHttpContextAccessor httpContextAccessor,
                ILanguageService languageService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _httpContextAccessor = httpContextAccessor;
                _languageService = languageService;
                _localizer = localizer;
            }

            private int GetRequiredCompanyId()
            {
                var context = _httpContextAccessor.HttpContext;
                var companyId = context?.Items["CompanyId"] as int?;
                if (!companyId.HasValue)
                    throw new UnauthorizedAccessException("Company ID is required in request header (X-CompanyId)");
                return companyId.Value;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();
                var lang = _languageService.GetCurrentLanguage();

                // التحقق من وجود الشركة
                var company = await _companyRepo.GetByIdAsync(companyId);
                if (company == null)
                    throw new NotFoundException("Create Dependant Type", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        companyId));

                // التحقق من عدم تكرار الكود
                if (await _repo.CodeExistsAsync(request.Data.Code, companyId))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("DependantType", lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.DependantType(
                    code: request.Data.Code,
                    companyId: companyId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    remarks: request.Data.Remarks,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.regComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}