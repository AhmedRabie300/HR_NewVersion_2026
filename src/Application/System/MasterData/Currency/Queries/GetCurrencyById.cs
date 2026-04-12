// Application/System/MasterData/Currency/Queries/GetCurrencyById.cs
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Currency.Queries
{
    public static class GetCurrencyById
    {
        public record Query(int Id) : IRequest<CurrencyDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly IContextService _ContextService;

            public Validator(ILocalizationService localizer, IContextService ContextService)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _ContextService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, CurrencyDto>
        {
            private readonly ICurrencyRepository _repo;
 
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ICurrencyRepository repo,
                IHttpContextAccessor httpContextAccessor,
                IContextService ContextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _ContextService = ContextService;
                _localizer = localizer;
            }

      
            public async Task<CurrencyDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Currency", lang),
                        request.Id));

                if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Currency does not belong to your company");

                return new CurrencyDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    CompanyId: entity.CompanyId,
                    CompanyName: entity.Company?.EngName ?? entity.Company?.ArbName,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    EngSymbol: entity.EngSymbol,
                    ArbSymbol: entity.ArbSymbol,
                    DecimalFraction: entity.DecimalFraction,
                    DecimalEngName: entity.DecimalEngName,
                    DecimalArbName: entity.DecimalArbName,
                    Amount: entity.Amount,
                    NoDecimalPlaces: entity.NoDecimalPlaces,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}