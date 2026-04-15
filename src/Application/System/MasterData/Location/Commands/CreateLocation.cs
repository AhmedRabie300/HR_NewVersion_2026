using Application.Common;
using Application.Common.Abstractions;
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
        public record Command(
            int CompanyId,
            int? RegUserId,
            CreateLocationDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                var lang = contextService.GetCurrentLanguage();
 
                RuleFor(x => x.Data)
                    .SetValidator(new CreateLocationValidator(localizer, contextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ILocationRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IBranchRepository _branchRepo;
            private readonly IDepartmentRepository _departmentRepo;
            private readonly ICityRepository _cityRepo;
            private readonly ICodeGenerationService _codeGenerationService;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ILocationRepository repo,
                ICompanyRepository companyRepo,
                IBranchRepository branchRepo,
                IDepartmentRepository departmentRepo,
                ICityRepository cityRepo,
                ICodeGenerationService codeGenerationService,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _branchRepo = branchRepo;
                _departmentRepo = departmentRepo;
                _cityRepo = cityRepo;
                _codeGenerationService = codeGenerationService;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var company = await _companyRepo.GetByIdAsync(request.CompanyId);
           
                if (request.Data.BranchId.HasValue)
                {
                    var branch = await _branchRepo.GetByIdAsync(request.Data.BranchId.Value);
                    if (branch == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("Branch", lang),
                            request.Data.BranchId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Branch", lang), request.Data.BranchId.Value));
                }

                if (request.Data.DepartmentId.HasValue)
                {
                    var department = await _departmentRepo.GetByIdAsync(request.Data.DepartmentId.Value);
                    if (department == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("Department", lang),
                            request.Data.DepartmentId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Department", lang), request.Data.DepartmentId.Value));
                }

                if (request.Data.CityId.HasValue)
                {
                    var city = await _cityRepo.GetByIdAsync(request.Data.CityId.Value);
                    if (city == null)
                        throw new NotFoundException(
                            _localizer.GetMessage("City", lang),
                            request.Data.CityId.Value,
                            string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("City", lang), request.Data.CityId.Value));
                }

                var code = await _codeGenerationService.GenerateCodeAsync(
                    request.CompanyId,
                    request.Data.Code,
                    (companyId, ct) => _repo.GetMaxCodeAsync(companyId, ct),
                    (code, ct) => _repo.CodeExistsAsync(code, request.CompanyId),
                    cancellationToken
                );

                var entity = new Domain.System.MasterData.Location(
                    code: code,
                    companyId: request.CompanyId,
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
                    regUserId: request.RegUserId,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}