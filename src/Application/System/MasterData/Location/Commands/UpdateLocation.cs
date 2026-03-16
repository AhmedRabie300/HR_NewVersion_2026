using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using Application.System.MasterData.Location.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Location.Commands
{
    public static class UpdateLocation
    {
        public record Command(UpdateLocationDto Data, int Lang = 1) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateLocationValidator(localization, lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Command, Unit>
        {
            private readonly ILocationRepository _repo;
            private readonly IBranchRepository _branchRepo;
            private readonly IDepartmentRepository _departmentRepo;

            public Handler(
                ILocationRepository repo,
                IBranchRepository branchRepo,
                IDepartmentRepository departmentRepo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
                _branchRepo = branchRepo;
                _departmentRepo = departmentRepo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var location = await _repo.GetByIdAsync(request.Data.Id);
                if (location == null)
                {
                    throw new NotFoundException(
                        GetMessage("Location", request.Lang),
                        request.Data.Id,
                        GetFormattedMessage("NotFound", request.Lang, GetMessage("Location", request.Lang), request.Data.Id)
                    );
                }

                // Check if branch exists if provided
                if (request.Data.BranchId.HasValue)
                {
                    var branch = await _branchRepo.GetByIdAsync(request.Data.BranchId.Value);
                    if (branch == null)
                    {
                        throw new NotFoundException(
                            GetMessage("Branch", request.Lang),
                            request.Data.BranchId.Value,
                            GetFormattedMessage("NotFound", request.Lang, GetMessage("Branch", request.Lang), request.Data.BranchId.Value)
                        );
                    }
                }

                // Check if department exists if provided
                if (request.Data.DepartmentId.HasValue)
                {
                    var department = await _departmentRepo.GetByIdAsync(request.Data.DepartmentId.Value);
                    if (department == null)
                    {
                        throw new NotFoundException(
                            GetMessage("Department", request.Lang),
                            request.Data.DepartmentId.Value,
                            GetFormattedMessage("NotFound", request.Lang, GetMessage("Department", request.Lang), request.Data.DepartmentId.Value)
                        );
                    }
                }

                // Update basic info
                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null)
                {
                    location.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );
                }

                // Update relations
                if (request.Data.CityId.HasValue ||
                    request.Data.BranchId.HasValue ||
                    request.Data.StoreId.HasValue ||
                    request.Data.DepartmentId.HasValue)
                {
                    location.UpdateRelations(
                        request.Data.CityId,
                        request.Data.BranchId,
                        request.Data.StoreId,
                        request.Data.DepartmentId
                    );
                }

                // Update cost centers
                if (request.Data.CostCenterCode1 != null ||
                    request.Data.CostCenterCode2 != null ||
                    request.Data.CostCenterCode3 != null ||
                    request.Data.CostCenterCode4 != null)
                {
                    location.UpdateCostCenters(
                        request.Data.CostCenterCode1,
                        request.Data.CostCenterCode2,
                        request.Data.CostCenterCode3,
                        request.Data.CostCenterCode4
                    );
                }

                // Update ledgers
                if (request.Data.InventoryCostLedgerId.HasValue ||
                    request.Data.InventoryAdjustmentLedgerId.HasValue)
                {
                    location.UpdateLedgers(
                        request.Data.InventoryCostLedgerId,
                        request.Data.InventoryAdjustmentLedgerId
                    );
                }

                await _repo.UpdateAsync(location);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}