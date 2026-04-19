using Application.Abstractions;
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
            public Validator(IValidationMessages msg)
            {
                }
        }

        public class Handler : IRequestHandler<Query, List<SectorDto>>
        {
            private readonly ISectorRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _contextService;
            private readonly ICurrentUser _currentUser;
            public Handler(
                ISectorRepository repo,
                ICompanyRepository companyRepo,
                ILocalizationService localizer, 
                IContextService contextService,
                ICurrentUser currentUser)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _contextService = contextService;
                _currentUser = currentUser;
            }

            public async Task<List<SectorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _contextService.GetCurrentCompanyId();
                var company = await _companyRepo.GetByIdAsync(companyId);

                var sectors = await _repo.GetByCompanyIdAsync(companyId);

                return sectors.Select(s => new SectorDto(
                    Id: s.Id,
                    Code: s.Code,
                    CompanyId: s.CompanyId,
                    CompanyName: _currentUser.Language == 2 ? company.ArbName : company.EngName,
                    EngName: s.EngName,
                    ArbName: s.ArbName,
                    ArbName4S: s.ArbName4S,
                    ParentId: s.ParentId,
                    ParentSectorName: _currentUser.Language == 2 ? s.ParentSector?.ArbName : s.ParentSector?.EngName,
                    Remarks: s.Remarks,
                    RegDate: s.RegDate,
                    CancelDate: s.CancelDate,
                    IsActive: s.IsActive()
                )).ToList();
            }
        }
    }
}