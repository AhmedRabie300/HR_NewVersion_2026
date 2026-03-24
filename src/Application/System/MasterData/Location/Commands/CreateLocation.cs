using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using Application.System.MasterData.Location.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Location.Commands
{
    public static class CreateLocation
    {
        public record Command(CreateLocationDto Data, int Lang = 1) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateLocationValidator(localization, lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Command, int>
        {
            private readonly ILocationRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IBranchRepository _branchRepo;
            private readonly IDepartmentRepository _departmentRepo;

            public Handler(
                ILocationRepository repo,
                ICompanyRepository companyRepo,
                IBranchRepository branchRepo,
                IDepartmentRepository departmentRepo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _branchRepo = branchRepo;
                _departmentRepo = departmentRepo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                // Check if company exists if provided
                if (request.Data.CompanyId.HasValue)
                {
                    var company = await _companyRepo.GetByIdAsync(request.Data.CompanyId.Value);
                    if (company == null)
                    {
                        throw new NotFoundException(
                            GetMessage("Company", request.Lang),
                            request.Data.CompanyId.Value,
                            GetFormattedMessage("NotFound", request.Lang, GetMessage("Company", request.Lang), request.Data.CompanyId.Value)
                        );
                    }
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

                // Check if code exists
                var exists = await _repo.CodeExistsAsync(request.Data.Code, request.Data.CompanyId);
                if (exists)
                {
                    throw new ConflictException(
                        GetMessage("Location", request.Lang),
                        GetMessage("Code", request.Lang),
                        request.Data.Code
                    );
                }

                var location = new Domain.System.MasterData.Location(
                    code: request.Data.Code,
                    companyId: request.Data.CompanyId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    cityId: request.Data.CityId,
                    branchId: request.Data.BranchId,
                    storeId: request.Data.StoreId,
                    inventoryCostLedgerId: request.Data.InventoryCostLedgerId,
                    inventoryAdjustmentLedgerId: request.Data.InventoryAdjustmentLedgerId,
                    departmentId: request.Data.DepartmentId,
                    remarks: request.Data.Remarks,
                    costCenterCode1: request.Data.CostCenterCode1,
                    costCenterCode2: request.Data.CostCenterCode2,
                    costCenterCode3: request.Data.CostCenterCode3,
                    costCenterCode4: request.Data.CostCenterCode4,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.regComputerId
                );

                await _repo.AddAsync(location);
                await _repo.SaveChangesAsync(cancellationToken);

                return location.Id;
            }
        }
    }
}