using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Country.Dtos;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Country.Queries
{
    public static class GetCountryById
    {
        public record Query(int Id) : IRequest<CountryDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, CountryDto>
        {
            private readonly ICountryRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(ICountryRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<CountryDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = 1;

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Country", request.Id));

                return new CountryDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    CurrencyId: entity.CurrencyId,
                    CurrencyName: lang == 2 ? entity.Currency?.ArbName : entity.Currency?.EngName,
                    NationalityId: entity.NationalityId,
                    NationalityName: lang == 2 ? entity.Nationality?.ArbName : entity.Nationality?.EngName,
                    PhoneKey: entity.PhoneKey,
                    IsMainCountries: entity.IsMainCountries,
                    Remarks: entity.Remarks,
                    RegionId: entity.RegionId,
                    RegionName: lang == 2 ? entity.Region?.ArbName : entity.Region?.EngName,
                    ISOAlpha2: entity.ISOAlpha2,
                    ISOAlpha3: entity.ISOAlpha3,
                    Languages: entity.Languages,
                    Continent: entity.Continent,
                    CapitalId: entity.CapitalId,
                    CapitalName: lang == 2 ? entity.Capital?.ArbName : entity.Capital?.EngName,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}