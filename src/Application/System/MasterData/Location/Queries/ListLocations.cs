using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using MediatR;

namespace Application.System.MasterData.Location.Queries
{
    public static class ListLocations
    {
        public record Query(int Lang = 1) : IRequest<List<LocationDto>>;

        public class Handler : IRequestHandler<Query, List<LocationDto>>
        {
            private readonly ILocationRepository _repo;

            public Handler(ILocationRepository repo)
            {
                _repo = repo;
            }

            public async Task<List<LocationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var locations = await _repo.GetAllAsync();

                return locations.Select(l => new LocationDto(
                    Id: l.Id,
                    Code: l.Code,
                    CompanyId: l.CompanyId,
                    CompanyName: request.Lang == 2 ? l.Company?.ArbName : l.Company?.EngName,
                    EngName: l.EngName,
                    ArbName: l.ArbName,
                    ArbName4S: l.ArbName4S,
                    CityId: l.CityId,
                    CityName: null,
                    BranchId: l.BranchId,
                    BranchName: request.Lang == 2 ? l.Branch?.ArbName : l.Branch?.EngName,
                    StoreId: l.StoreId,
                    StoreName: null,
                    InventoryCostLedgerId: l.InventoryCostLedgerId,
                    InventoryCostLedgerName: null,
                    InventoryAdjustmentLedgerId: l.InventoryAdjustmentLedgerId,
                    InventoryAdjustmentLedgerName: null,
                    DepartmentId: l.DepartmentId,
                    DepartmentName: request.Lang == 2 ? l.Department?.ArbName : l.Department?.EngName,
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