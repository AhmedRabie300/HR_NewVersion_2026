// Application/System/MasterData/Location/Commands/CreateLocation.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using Application.System.MasterData.Location.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Location.Commands
{
    public static class CreateLocation
    {
        public record Command(CreateLocationDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                _ContextService = ContextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new CreateLocationValidator(_localizer, _ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ILocationRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IBranchRepository _branchRepo;
            private readonly IDepartmentRepository _departmentRepo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ILocationRepository repo,
                ICompanyRepository companyRepo,
                IBranchRepository branchRepo,
                IDepartmentRepository departmentRepo,
                IContextService ContextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _branchRepo = branchRepo;
                _departmentRepo = departmentRepo;
                _ContextService = ContextService;                
_localizer = localizer;
            }


            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                // التحقق من وجود الشركة
                var company = await _companyRepo.GetByIdAsync(companyId);
                if (company == null)
                    throw new NotFoundException("Create Location", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Company", lang),
                        companyId));

                // التحقق من وجود الفرع إذا وجد
                if (request.Data.BranchId.HasValue)
                {
                    var branch = await _branchRepo.GetByIdAsync(request.Data.BranchId.Value);
                    if (branch == null)
                        throw new NotFoundException("Create Location", string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("Branch", lang),
                            request.Data.BranchId));
                }

                // التحقق من وجود القسم إذا وجد
                if (request.Data.DepartmentId.HasValue)
                {
                    var department = await _departmentRepo.GetByIdAsync(request.Data.DepartmentId.Value);
                    if (department == null)
                        throw new NotFoundException("Create Location", string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("Department", lang),
                            request.Data.DepartmentId));
                }

                // التحقق من عدم تكرار الكود
                if (await _repo.CodeExistsAsync(request.Data.Code, companyId))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("Location", lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.Location(
                    code: request.Data.Code,
                    companyId: companyId,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    cityId: request.Data.CityId,
                    branchId: request.Data.BranchId,
                    storeId: request.Data.StoreId,
                    departmentId: request.Data.DepartmentId,
                    remarks: request.Data.Remarks,
                    costCenterCode1: request.Data.CostCenterCode1,
                    costCenterCode2: request.Data.CostCenterCode2,
                    costCenterCode3: request.Data.CostCenterCode3,
                    costCenterCode4: request.Data.CostCenterCode4,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.regComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}