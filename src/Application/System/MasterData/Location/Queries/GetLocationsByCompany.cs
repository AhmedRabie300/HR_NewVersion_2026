using Application.Abstractions;
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

        
        public class Handler : IRequestHandler<Query, List<LocationDto>>
        {
            private readonly ILocationRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;
            private readonly ICurrentUser _currentUser;
            public Handler(
                ILocationRepository repo,
                ICompanyRepository companyRepo,
                IContextService contextService,
                ILocalizationService localizer,
                ICurrentUser currentUser)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _contextService = contextService;
                _localizer = localizer;
                _currentUser = currentUser;
            }

            public async Task<List<LocationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
 
                var locations = await _repo.GetByCompanyIdAsync();

                return locations.Select(l => new LocationDto(
                    Id: l.Id,
                    Code: l.Code,
                    CompanyId: l.CompanyId,
                    EngName: l.EngName,
                    ArbName: l.ArbName,
                    ArbName4S: l.ArbName4S,
                    CityId: l.CityId,
                    CityName: null,
                    BranchId: l.BranchId,
                    BranchName: _currentUser.Language == 2 ? l.Branch?.ArbName : l.Branch?.EngName,
                    StoreId: l.StoreId,
                    StoreName: null,
                    InventoryCostLedgerId: l.InventoryCostLedgerId,
                    InventoryCostLedgerName: null,
                    InventoryAdjustmentLedgerId: l.InventoryAdjustmentLedgerId,
                    InventoryAdjustmentLedgerName: null,
                    DepartmentId: l.DepartmentId,
                    DepartmentName: _currentUser.Language == 2 ? l.Department?.ArbName : l.Department?.EngName,
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