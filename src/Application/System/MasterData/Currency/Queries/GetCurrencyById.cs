// Application/System/MasterData/Currency/Queries/GetCurrencyById.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Currency.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Currency.Queries
{
    public static class GetCurrencyById
    {
        public record Query(int Id) : IRequest<CurrencyDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, CurrencyDto>
        {
            private readonly ICurrencyRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                ICurrencyRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<CurrencyDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Currency", request.Id));

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