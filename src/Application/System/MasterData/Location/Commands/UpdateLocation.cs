using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Location.Dtos;
using Application.System.MasterData.Location.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Location.Commands
{
    public static class UpdateLocation
    {
        public record Command(UpdateLocationDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateLocationValidator(localizer, languageService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ILocationRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IBranchRepository _branchRepo;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(ILocationRepository repo, ICompanyRepository companyRepo, IBranchRepository branchRepo, ILanguageService languageService, ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _branchRepo = branchRepo;
                _languageService = languageService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _languageService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Location", lang),
                        request.Data.Id));

                // Update basic info (name, remarks)
                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null)
                {
                    entity.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );
                }

                // Update cost centers
                if (request.Data.CostCenterCode1 != null ||
                    request.Data.CostCenterCode2 != null ||
                    request.Data.CostCenterCode3 != null ||
                    request.Data.CostCenterCode4 != null)
                {
                    entity.UpdateCostCenters(
                        request.Data.CostCenterCode1,
                        request.Data.CostCenterCode2,
                        request.Data.CostCenterCode3,
                        request.Data.CostCenterCode4
                    );
                }

                // Update parent
                if (request.Data.ParentId.HasValue)
                {
                    if (request.Data.ParentId != entity.Id)
                    {
                        var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                        if (parent == null)
                            throw new Exception(string.Format(
                                _localizer.GetMessage("NotFound", lang),
                                _localizer.GetMessage("ParentLocation", lang),
                                request.Data.ParentId));

                        // Assuming there's an UpdateParent method
                        // entity.UpdateParent(request.Data.ParentId);
                    }
                }

                // Update company
                if (request.Data.CompanyId.HasValue)
                {
                    var company = await _companyRepo.GetByIdAsync(request.Data.CompanyId.Value);
                    if (company == null)
                        throw new Exception(string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("Company", lang),
                            request.Data.CompanyId));
                    // entity.UpdateCompany(request.Data.CompanyId);
                }

                // Update branch
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