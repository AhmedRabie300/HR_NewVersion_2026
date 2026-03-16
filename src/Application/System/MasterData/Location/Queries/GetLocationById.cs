using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Location.Queries
{
    public static class GetLocationById
    {
        public record Query(int Id, int Lang = 1) : IRequest<LocationDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(localization.GetMessage("IdGreaterThanZero", lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Query, LocationDto>
        {
            private readonly ILocationRepository _repo;

            public Handler(
                ILocationRepository repo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
            }

            public async Task<LocationDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var location = await _repo.GetByIdAsync(request.Id);
                if (location == null)
                {
                    throw new NotFoundException(
                        GetMessage("Location", request.Lang),
                        request.Id,
                        GetFormattedMessage("NotFound", request.Lang, GetMessage("Location", request.Lang), request.Id)
                    );
                }

                return new LocationDto(
                    Id: location.Id,
                    Code: location.Code,
                    CompanyId: location.CompanyId,
                    CompanyName: request.Lang == 2 ? location.Company?.ArbName : location.Company?.EngName,
                    EngName: location.EngName,
                    ArbName: location.ArbName,
                    ArbName4S: location.ArbName4S,
                    CityId: location.CityId,
                    CityName: null,  
                    BranchId: location.BranchId,
                    BranchName: request.Lang == 2 ? location.Branch?.ArbName : location.Branch?.EngName,
                    StoreId: location.StoreId,
                    StoreName: null,  
                    InventoryCostLedgerId: location.InventoryCostLedgerId,
                    InventoryCostLedgerName: null,  
                    InventoryAdjustmentLedgerId: location.InventoryAdjustmentLedgerId,
                    InventoryAdjustmentLedgerName: null,  
                    DepartmentId: location.DepartmentId,
                    DepartmentName: request.Lang == 2 ? location.Department?.ArbName : location.Department?.EngName,
                    Remarks: location.Remarks,
                    CostCenterCode1: location.CostCenterCode1,
                    CostCenterCode2: location.CostCenterCode2,
                    CostCenterCode3: location.CostCenterCode3,
                    CostCenterCode4: location.CostCenterCode4,
                    RegDate: location.RegDate,
                    CancelDate: location.CancelDate,
                    IsActive: location.IsActive()
                );
            }
        }
    }
}