using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Location.Queries
{
    public static class GetLocationsByCompany
    {
        public record Query(int CompanyId, int Lang = 1) : IRequest<List<LocationDto>>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.CompanyId)
                    .GreaterThan(0).WithMessage(localization.GetMessage("IdGreaterThanZero", lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Query, List<LocationDto>>
        {
            private readonly ILocationRepository _repo;
            private readonly ICompanyRepository _companyRepo;

            public Handler(
                ILocationRepository repo,
                ICompanyRepository companyRepo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
                _companyRepo = companyRepo;
            }

            public async Task<List<LocationDto>> Handle(Query request, CancellationToken cancellationToken)
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

                var locations = await _repo.GetByCompanyIdAsync(request.CompanyId);

                return locations.Select(l => new LocationDto(
                    Id: l.Id,
                    Code: l.Code,
                    CompanyId: l.CompanyId,
                    CompanyName: request.Lang == 2 ? company.ArbName : company.EngName,
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