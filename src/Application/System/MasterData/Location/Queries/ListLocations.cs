// Application/System/MasterData/Location/Queries/ListLocations.cs
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Location.Queries
{
    public static class ListLocations
    {
        public record Query : IRequest<List<LocationDto>>;

        public class Handler : IRequestHandler<Query, List<LocationDto>>
        {
            private readonly ILocationRepository _repo;
            private readonly IContextService _ContextService;

            public Handler(ILocationRepository repo, IContextService ContextService)
            {
                _repo = repo;
                _ContextService = ContextService;
            }


            public async Task<List<LocationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                var items = await _repo.GetAllAsync(companyId);

                return items.Select(x => new LocationDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName ?? x.Company?.ArbName,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    CityId: x.CityId,
                    CityName: null,  // CityName - سيتم لاحقاً
                    BranchId: x.BranchId,
                    BranchName: x.Branch?.EngName ?? x.Branch?.ArbName,
                    StoreId: x.StoreId,
                    StoreName: null,  // StoreName - سيتم لاحقاً
                    InventoryCostLedgerId: x.InventoryCostLedgerId,
                    InventoryCostLedgerName: null,  // سيتم لاحقاً
                    InventoryAdjustmentLedgerId: x.InventoryAdjustmentLedgerId,
                    InventoryAdjustmentLedgerName: null,  // سيتم لاحقاً
                    DepartmentId: x.DepartmentId,
                    DepartmentName: x.Department?.EngName ?? x.Department?.ArbName,
                    Remarks: x.Remarks,
                    CostCenterCode1: x.CostCenterCode1,
                    CostCenterCode2: x.CostCenterCode2,
                    CostCenterCode3: x.CostCenterCode3,
                    CostCenterCode4: x.CostCenterCode4,
                    RegDate: x.RegDate,
                    CancelDate: x.CancelDate,
                    IsActive: x.IsActive()
                )).ToList();
            }
        }
    }
}