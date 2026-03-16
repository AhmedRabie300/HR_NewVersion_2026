using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
 using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sector.Queries
{
    public static class GetSectorById
    {
        public record Query(int Id, int Lang = 1) : IRequest<SectorDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(localization.GetMessage("IdGreaterThanZero", lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Query, SectorDto>
        {
            private readonly ISectorRepository _repo;

            public Handler(
                ISectorRepository repo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
            }

            public async Task<SectorDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var sector = await _repo.GetByIdAsync(request.Id);
                if (sector == null)
                {
                    throw new NotFoundException(
                        GetMessage("Sector", request.Lang),
                        request.Id,
                        GetFormattedMessage("NotFound", request.Lang, GetMessage("Sector", request.Lang), request.Id)
                    );
                }

                return new SectorDto(
                    Id: sector.Id,
                    Code: sector.Code,
                    CompanyId: sector.CompanyId,
                    CompanyName: request.Lang == 2 ? sector.Company?.ArbName : sector.Company?.EngName,
                    EngName: sector.EngName,
                    ArbName: sector.ArbName,
                    ArbName4S: sector.ArbName4S,
                    ParentId: sector.ParentId,
                    ParentSectorName: request.Lang == 2 ? sector.ParentSector?.ArbName : sector.ParentSector?.EngName,
                    Remarks: sector.Remarks,
                    RegDate: sector.RegDate,
                    CancelDate: sector.CancelDate,
                    IsActive: sector.IsActive()
                );
            }
        }
    }
}