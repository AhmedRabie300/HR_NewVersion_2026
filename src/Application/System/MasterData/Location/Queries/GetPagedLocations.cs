// Application/System/MasterData/Location/Queries/GetPagedLocations.cs
using Application.Common.Abstractions;
using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Location.Queries
{
    public static class GetPagedLocations
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm)
            : IRequest<PagedResult<LocationDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<LocationDto>>
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
                    throw new UnauthorizedAccessException("Company ID is required in request header");
                return companyId.Value;
            }

            public async Task<PagedResult<LocationDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();
                var lang = _languageService.GetCurrentLanguage();

                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    companyId
                );

                var items = pagedResult.Items.Select(x => new LocationDto(
                    Id: x.Id,
                    Code: x.Code,
                    CompanyId: x.CompanyId,
                    CompanyName: x.Company?.EngName ?? x.Company?.ArbName,
                    EngName: x.EngName,
                    ArbName: x.ArbName,
                    ArbName4S: x.ArbName4S,
                    CityId: x.CityId,
                    CityName: null,
                    BranchId: x.BranchId,
                    BranchName: x.Branch?.EngName ?? x.Branch?.ArbName,
                    StoreId: x.StoreId,
                    StoreName: null,
                    InventoryCostLedgerId: x.InventoryCostLedgerId,
                    InventoryCostLedgerName: null,
                    InventoryAdjustmentLedgerId: x.InventoryAdjustmentLedgerId,
                    InventoryAdjustmentLedgerName: null,
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

                return new PagedResult<LocationDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}