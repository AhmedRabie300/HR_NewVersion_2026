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
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILanguageService _languageService;

            public Handler(ILocationRepository repo, IHttpContextAccessor httpContextAccessor, ILanguageService languageService)
            {
                _repo = repo;
                _httpContextAccessor = httpContextAccessor;
                _languageService = languageService;
            }

            private int GetRequiredCompanyId()
            {
                var context = _httpContextAccessor.HttpContext;
                var companyId = context?.Items["CompanyId"] as int?;
                if (!companyId.HasValue)
                    throw new UnauthorizedAccessException("Company ID is required in request header (X-CompanyId)");
                return companyId.Value;
            }

            public async Task<List<LocationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();
                var lang = _languageService.GetCurrentLanguage();

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