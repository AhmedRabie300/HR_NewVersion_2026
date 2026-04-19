// Application/System/MasterData/Location/Queries/GetLocationById.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Location.Queries
{
    public static class GetLocationById
    {
        public record Query(int Id) : IRequest<LocationDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, LocationDto>
        {
            private readonly ILocationRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                ILocationRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<LocationDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Location", request.Id));

                return new LocationDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    CompanyId: entity.CompanyId,
                    CompanyName: entity.Company?.EngName ?? entity.Company?.ArbName,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    CityId: entity.CityId,
                    CityName: null,
                    BranchId: entity.BranchId,
                    BranchName: entity.Branch?.EngName ?? entity.Branch?.ArbName,
                    StoreId: entity.StoreId,
                    StoreName: null,
                    InventoryCostLedgerId: entity.InventoryCostLedgerId,
                    InventoryCostLedgerName: null,
                    InventoryAdjustmentLedgerId: entity.InventoryAdjustmentLedgerId,
                    InventoryAdjustmentLedgerName: null,
                    DepartmentId: entity.DepartmentId,
                    DepartmentName: entity.Department?.EngName ?? entity.Department?.ArbName,
                    Remarks: entity.Remarks,
                    CostCenterCode1: entity.CostCenterCode1,
                    CostCenterCode2: entity.CostCenterCode2,
                    CostCenterCode3: entity.CostCenterCode3,
                    CostCenterCode4: entity.CostCenterCode4,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}