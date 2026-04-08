// Application/System/MasterData/Location/Commands/UpdateLocation.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using Application.System.MasterData.Location.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Location.Commands
{
    public static class UpdateLocation
    {
        public record Command(UpdateLocationDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateLocationValidator(_localizer, _languageService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ILocationRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IBranchRepository _branchRepo;
            private readonly IDepartmentRepository _departmentRepo;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ILocationRepository repo,
                ICompanyRepository companyRepo,
                IBranchRepository branchRepo,
                IDepartmentRepository departmentRepo,
                IHttpContextAccessor httpContextAccessor,
                ILanguageService languageService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _branchRepo = branchRepo;
                _departmentRepo = departmentRepo;
                _httpContextAccessor = httpContextAccessor;
                _languageService = languageService;
                _localizer = localizer;
            }

            private int GetRequiredCompanyId()
            {
                var context = _httpContextAccessor.HttpContext;
                var companyId = context?.Items["CompanyId"] as int?;
                if (!companyId.HasValue)
                    throw new UnauthorizedAccessException("Company ID is required in request header");
                return companyId.Value;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = GetRequiredCompanyId();
                var lang = _languageService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Location", lang),
                        request.Data.Id));

                // التأكد أن الموقع يتبع الشركة الحالية
                if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Location does not belong to your company");

                // Update basic info
                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null ||
                    request.Data.CostCenterCode1 != null ||
                    request.Data.CostCenterCode2 != null ||
                    request.Data.CostCenterCode3 != null ||
                    request.Data.CostCenterCode4 != null)
                {
                    entity.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks,
                        request.Data.CostCenterCode1,
                        request.Data.CostCenterCode2,
                        request.Data.CostCenterCode3,
                        request.Data.CostCenterCode4
                    );
                }

                // Update parent
                if (request.Data.ParentId.HasValue && request.Data.ParentId != entity.Id)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new Exception(string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("ParentLocation", lang),
                            request.Data.ParentId));
                    // entity.UpdateParent(request.Data.ParentId);
                }

                // Update relations
                if (request.Data.BranchId.HasValue)
                {
                    var branch = await _branchRepo.GetByIdAsync(request.Data.BranchId.Value);
                    if (branch == null)
                        throw new Exception(string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("Branch", lang),
                            request.Data.BranchId));
                    // entity.UpdateBranch(request.Data.BranchId);
                }

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}