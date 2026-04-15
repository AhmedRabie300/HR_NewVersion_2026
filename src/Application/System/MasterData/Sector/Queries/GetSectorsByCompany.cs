using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sector.Queries
{
    public static class GetSectorsByCompany
    {
        public record Query : IRequest<List<SectorDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;
             }
        }

        public class Handler : IRequestHandler<Query, List<SectorDto>>
        {
            private readonly ISectorRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ISectorRepository repo,
                ICompanyRepository companyRepo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<List<SectorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _contextService.GetCurrentCompanyId();
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(companyId);
       
                var sectors = await _repo.GetByCompanyIdAsync(companyId);

                return sectors.Select(s => new SectorDto(
                    Id: s.Id,
                    Code: s.Code,
                    CompanyId: s.CompanyId,
                    CompanyName: lang == 2 ? company.ArbName : company.EngName,
                    EngName: s.EngName,
                    ArbName: s.ArbName,
                    ArbName4S: s.ArbName4S,
                    ParentId: s.ParentId,
                    ParentSectorName: lang == 2 ? s.ParentSector?.ArbName : s.ParentSector?.EngName,
                    Remarks: s.Remarks,
                    RegDate: s.RegDate,
                    CancelDate: s.CancelDate,
                    IsActive: s.IsActive()
                )).ToList();
            }
        }
    }
}