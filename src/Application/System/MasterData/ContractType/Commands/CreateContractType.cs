// Application/System/MasterData/ContractType/Commands/CreateContractType.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.ContractType.Dtos;
using Application.System.MasterData.ContractType.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.ContractType.Commands
{
    public static class CreateContractType
    {
        public record Command(CreateContractTypeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                _ContextService = ContextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new CreateContractTypeValidator(_localizer, _ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IContractTypeRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IContractTypeRepository repo,
                ICompanyRepository companyRepo,
                IHttpContextAccessor httpContextAccessor,
                IContextService ContextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _httpContextAccessor = httpContextAccessor;
                _ContextService = ContextService;
                _localizer = localizer;
            }

       

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                 var company = await _companyRepo.GetByIdAsync(companyId);
                if (company == null)
                    throw new NotFoundException("Create Contract Type", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        companyId));

                 if (await _repo.CodeExistsAsync(request.Data.Code, companyId))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("ContractType", lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.ContractType(
                    request.Data.Code,
                    companyId,
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