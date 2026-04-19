using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Region.Dtos;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Region.Queries
{
    public static class GetRegionById
    {
        public record Query(int Id) : IRequest<RegionDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, RegionDto>
        {
            private readonly IRegionRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(IRegionRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<RegionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = 1;

                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Region", request.Id));

                return new RegionDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    CountryId: entity.CountryId,
                    CountryName: lang == 2 ? entity.Country?.ArbName : entity.Country?.EngName,
                    CompanyId: entity.CompanyId,
                    CompanyName: entity.Company?.EngName,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}