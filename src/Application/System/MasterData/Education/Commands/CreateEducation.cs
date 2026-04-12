// Application/System/MasterData/Education/Commands/CreateEducation.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Education.Dtos;
using Application.System.MasterData.Education.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Education.Commands
{
    public static class CreateEducation
    {
        public record Command(CreateEducationDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                _ContextService = ContextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new CreateEducationValidator(_localizer, _ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IEducationRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IEducationRepository repo,
                ICompanyRepository companyRepo,
                IContextService ContextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _ContextService = ContextService;
                _localizer = localizer;
            }
 
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                // التحقق من وجود الشركة
                var company = await _companyRepo.GetByIdAsync(companyId);
                if (company == null)
                    throw new NotFoundException("Create Education", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        companyId));

                // التحقق من عدم تكرار الكود
                if (await _repo.CodeExistsAsync(request.Data.Code, companyId))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("Education", lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.Education(
                    code: request.Data.Code,
                    companyId: companyId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    level: request.Data.Level,
                    requiredYears: request.Data.RequiredYears,
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