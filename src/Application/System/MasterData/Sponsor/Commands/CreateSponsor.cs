using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using Application.System.MasterData.Sponsor.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Sponsor.Commands
{
    public static class CreateSponsor
    {
        public record Command(CreateSponsorDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                _ContextService = ContextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new CreateSponsorValidator(_localizer, _ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
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

      

       
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                 var company = await _companyRepo.GetByIdAsync(companyId);
                if (company == null)
                    throw new NotFoundException("Create Sponsor", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        companyId));

                // التحقق من عدم تكرار الكود داخل نفس الشركة
                if (await _repo.CodeExistsAsync(request.Data.Code, companyId))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("Sponsor", lang),
                        request.Data.Code));

                
                var entity = new Domain.System.MasterData.Sponsor(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    sponsorNumber: request.Data.SponsorNumber,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.RegComputerId,
                    companyId: companyId  
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}