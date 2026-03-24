using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Currency.Queries
{
    public static class GetCurrencyById
    {
        public record Query(int Id, int Lang = 1) : IRequest<CurrencyDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Query, CurrencyDto>
        {
            private readonly ICurrencyRepository _repo;

            public Handler(ICurrencyRepository repo)
            {
                _repo = repo;
            }

            public async Task<CurrencyDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new Exception($"Currency with ID {request.Id} not found");

                return new CurrencyDto(
                    entity.Id,
                    entity.Code,
                    entity.EngName,
                    entity.ArbName,
                    entity.ArbName4S,
                    entity.EngSymbol,
                    entity.ArbSymbol,
                    entity.DecimalFraction,
                    entity.DecimalEngName,
                    entity.DecimalArbName,
                    entity.Amount,
                    entity.NoDecimalPlaces,
                    entity.CompanyId,
                    entity.Company?.EngName ?? entity.Company?.ArbName,
                    entity.Remarks,
                    entity.RegDate,
                    entity.CancelDate,
                    entity.IsActive()
                );
            }
        }
    }
}