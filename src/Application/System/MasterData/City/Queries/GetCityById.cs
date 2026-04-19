using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.City.Dtos;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.City.Queries
{
    public static class GetCityById
    {
        public record Query(int Id) : IRequest<CityDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, CityDto>
        {
            private readonly ICityRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(ICityRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<CityDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = 1;

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("City", request.Id));

                return new CityDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    PhoneKey: entity.PhoneKey,
                    RegionId: entity.RegionId,
                    RegionName: lang == 2 ? entity.Region?.ArbName : entity.Region?.EngName,
                    TimeZone: entity.TimeZone,
                    CountryId: entity.CountryId,
                    CountryName: lang == 2 ? entity.Country?.ArbName : entity.Country?.EngName,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}