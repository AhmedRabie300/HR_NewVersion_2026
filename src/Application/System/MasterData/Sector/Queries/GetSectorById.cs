// Application/System/MasterData/Sector/Queries/GetSectorById.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sector.Queries
{
    public static class GetSectorById
    {
        public record Query(int Id) : IRequest<SectorDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, SectorDto>
        {
            private readonly ISectorRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                ISectorRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<SectorDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Sector", request.Id));

                return new SectorDto(
                    Id: entity.Id,
                    Code: entity.Code,
                    CompanyId: entity.CompanyId,
                    EngName: entity.EngName,
                    ArbName: entity.ArbName,
                    ArbName4S: entity.ArbName4S,
                    ParentId: entity.ParentId,
                    ParentSectorName: entity.ParentSector?.EngName ?? entity.ParentSector?.ArbName,
                    Remarks: entity.Remarks,
                    RegDate: entity.RegDate,
                    CancelDate: entity.CancelDate,
                    IsActive: entity.IsActive()
                );
            }
        }
    }
}