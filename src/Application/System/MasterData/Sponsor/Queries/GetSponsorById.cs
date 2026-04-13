using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Sponsor.Queries
{
    public static class GetSponsorById
    {
        public record Query(int Id) : IRequest<SponsorDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly IContextService _ContextService;

            public Validator(ILocalizationService localizer, IContextService ContextService)
            {
                _localizer = localizer;
                _ContextService = ContextService;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _ContextService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, SponsorDto>
        {
            private readonly ISponsorRepository _repo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;  

            public Handler(
                ISponsorRepository repo,
                IContextService ContextService,
                ILocalizationService localizer)  
            {
                _repo = repo;
                _ContextService = ContextService;
                _localizer = localizer;
            }


            public async Task<SponsorDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException("NotFound", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Sponsor", lang),
                        request.Id));

                // التأكد أن الكفيل تابع للشركة الحالية
                if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Sponsor does not belong to your company");

                return new SponsorDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    CompanyId: entity.CompanyId,
                    CompanyName: entity.Company?.EngName ?? entity.Company?.ArbName,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    SponsorNumber: entity.SponsorNumber,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}