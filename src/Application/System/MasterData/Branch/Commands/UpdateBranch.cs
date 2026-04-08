using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using Application.System.MasterData.Branch.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Branch.Commands
{
    public static class UpdateBranch
    {
        public record Command(UpdateBranchDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateBranchValidator(localizer, languageService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IBranchRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IBranchRepository repo,
                IHttpContextAccessor httpContextAccessor,
                ILanguageService languageService,
                ILocalizationService localizer)
            {
                _repo = repo;
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

                var branch = await _repo.GetByIdAsync(request.Data.Id);
                if (branch == null)
                    throw new NotFoundException("Update Branch", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Branch", lang),
                        request.Data.Id));

                // ✅ التأكد أن الفرع يتبع الشركة الحالية
                if (branch.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Branch does not belong to your company");

                // Update basic info
                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null)
                {
                    branch.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );
                }

                // Update location
                if (request.Data.CountryId.HasValue || request.Data.CityId.HasValue)
                {
                    branch.UpdateLocation(
                        request.Data.CountryId,
                        request.Data.CityId
                    );
                }

                // Update parent
                if (request.Data.ParentId.HasValue)
                {
                    if (request.Data.ParentId != branch.Id)
                    {
                        var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                        if (parent == null)
                            throw new NotFoundException("Update Branch", string.Format(
                                _localizer.GetMessage("NotFound", lang),
                                _localizer.GetMessage("ParentBranch", lang),
                                request.Data.ParentId));

                        branch.UpdateParent(request.Data.ParentId);
                    }
                }

                // Update settings
                if (request.Data.DefaultAbsent.HasValue ||
                    request.Data.PrepareDay.HasValue ||
                    request.Data.AffectPeriod.HasValue)
                {
                    branch.UpdateSettings(
                        request.Data.DefaultAbsent,
                        request.Data.PrepareDay,
                        request.Data.AffectPeriod
                    );
                }

                await _repo.UpdateAsync(branch);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}