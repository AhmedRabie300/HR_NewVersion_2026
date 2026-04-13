using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Location.Queries
{
    public static class GetLocationsByCompany
    {
        public record Query : IRequest<List<LocationDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;
                // لا توجد قواعد إضافية
            }
        }

        public class Handler : IRequestHandler<Query, List<LocationDto>>
        {
            private readonly ILocationRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ILocationRepository repo,
                ICompanyRepository companyRepo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<List<LocationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _contextService.GetCurrentCompanyId();
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(companyId);
                if (company == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Company", lang),
                        companyId,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Company", lang), companyId));

                var locations = await _repo.GetByCompanyIdAsync(companyId);

                return locations.Select(l => new LocationDto(
                    Id: l.Id,
                    Code: l.Code,
                    CompanyId: l.CompanyId,
                    CompanyName: lang == 2 ? company.ArbName : company.EngName,
                    EngName: l.EngName,
                    ArbName: l.ArbName,
                    ArbName4S: l.ArbName4S,
                    CityId: l.CityId,
                    CityName: null,
                    BranchId: l.BranchId,
                    BranchName: lang == 2 ? l.Branch?.ArbName : l.Branch?.EngName,
                    StoreId: l.StoreId,
                    StoreName: null,
                    InventoryCostLedgerId: l.InventoryCostLedgerId,
                    InventoryCostLedgerName: null,
                    InventoryAdjustmentLedgerId: l.InventoryAdjustmentLedgerId,
                    InventoryAdjustmentLedgerName: null,
                    DepartmentId: l.DepartmentId,
                    DepartmentName: lang == 2 ? l.Department?.ArbName : l.Department?.EngName,
                    Remarks: l.Remarks,
                    CostCenterCode1: l.CostCenterCode1,
                    CostCenterCode2: l.CostCenterCode2,
                    CostCenterCode3: l.CostCenterCode3,
                    CostCenterCode4: l.CostCenterCode4,
                    RegDate: l.RegDate,
                    CancelDate: l.CancelDate,
                    IsActive: l.IsActive()
                )).ToList();
            }
        }
    }
}