// Application/System/MasterData/Currency/Commands/UpdateCurrency.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using Application.System.MasterData.Currency.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Currency.Commands
{
    public static class UpdateCurrency
    {
        public record Command(UpdateCurrencyDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateCurrencyValidator(_localizer, _languageService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ICurrencyRepository _repo;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ICurrencyRepository repo,
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

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Currency", lang),
                        request.Data.Id));

                // التأكد أن العملة تتبع الشركة الحالية
                if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Currency does not belong to your company");

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.EngSymbol,
                    request.Data.ArbSymbol,
                    request.Data.DecimalFraction,
                    request.Data.DecimalEngName,
                    request.Data.DecimalArbName,
                    request.Data.Amount,
                    request.Data.NoDecimalPlaces,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}