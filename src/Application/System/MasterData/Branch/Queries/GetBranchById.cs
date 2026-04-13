using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Branch.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Branch.Queries
{
    public static class GetBranchById
    {
        public record Query(int Id) : IRequest<BranchDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly IContextService _contextService;

            public Validator(ILocalizationService localizer, IContextService contextService)
            {
                _localizer = localizer;
                _contextService = contextService;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _contextService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, BranchDto>
        {
            private readonly IBranchRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IBranchRepository repo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<BranchDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _contextService.GetCurrentCompanyId();
                var lang = _contextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException("NotFound",string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Branch", lang),
                        request.Id));

                if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Branch does not belong to your company");

                return new BranchDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    CompanyId: companyId,
                    CompanyName: entity.Company?.EngName ?? entity.Company?.ArbName,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    ParentId: entity.ParentId,
                    ParentBranchName: entity.ParentBranch?.EngName ?? entity.ParentBranch?.ArbName,
                    CountryId: entity.CountryId,
                    CityId: entity.CityId,
                    DefaultAbsent: entity.DefaultAbsent,
                    PrepareDay: entity.PrepareDay,
                    AffectPeriod: entity.AffectPeriod,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}