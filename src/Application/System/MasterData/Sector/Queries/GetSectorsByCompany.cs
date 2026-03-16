using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sector.Queries
{
    public static class GetSectorsByCompany
    {
        public record Query(int CompanyId, int Lang = 1) : IRequest<List<SectorDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.CompanyId)
                    .GreaterThan(0).WithMessage(localization.GetMessage("IdGreaterThanZero", lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Query, List<SectorDto>>
        {
            private readonly ISectorRepository _repo;
            private readonly ICompanyRepository _companyRepo;

            public Handler(
                ISectorRepository repo,
                ICompanyRepository companyRepo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
                _companyRepo = companyRepo;
            }

            public async Task<List<SectorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var company = await _companyRepo.GetByIdAsync(request.CompanyId);
                if (company == null)
                {
                    throw new NotFoundException(
                        GetMessage("Company", request.Lang),
                        request.CompanyId,
                        GetFormattedMessage("NotFound", request.Lang, GetMessage("Company", request.Lang), request.CompanyId)
                    );
                }

                var sectors = await _repo.GetByCompanyIdAsync(request.CompanyId);

                return sectors.Select(s => new SectorDto(
                    Id: s.Id,
                    Code: s.Code,
                    CompanyId: s.CompanyId,
                    CompanyName: request.Lang == 2 ? company.ArbName : company.EngName,
                    EngName: s.EngName,
                    ArbName: s.ArbName,
                    ArbName4S: s.ArbName4S,
                    ParentId: s.ParentId,
                    ParentSectorName: request.Lang == 2 ? s.ParentSector?.ArbName : s.ParentSector?.EngName,
                    Remarks: s.Remarks,
                    RegDate: s.RegDate,
                    CancelDate: s.CancelDate,
                    IsActive: s.IsActive()
                )).ToList();
            }
        }
    }
}