using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using Application.System.MasterData.Sector.Validators;
using Domain.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Sector.Commands
{
    public static class UpdateSector
    {
        public record Command(UpdateSectorDto Data, int Lang = 1) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateSectorValidator(localization, lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Command, Unit>
        {
            private readonly ISectorRepository _repo;

            public Handler(
                ISectorRepository repo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var sector = await _repo.GetByIdAsync(request.Data.Id);
                if (sector == null)
                {
                    throw new NotFoundException(
                        GetMessage("Sector", request.Lang),
                        request.Data.Id,
                        GetFormattedMessage("NotFound", request.Lang, GetMessage("Sector", request.Lang), request.Data.Id)
                    );
                }

                // Update basic info
                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null)
                {
                    sector.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );
                }

                // Update parent
                if (request.Data.ParentId.HasValue)
                {
                    // Check if parent exists and not self-reference
                    if (request.Data.ParentId != sector.Id)
                    {
                        var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                        if (parent == null)
                        {
                            throw new NotFoundException(
                                GetMessage("ParentSector", request.Lang),
                                request.Data.ParentId.Value,
                                GetFormattedMessage("NotFound", request.Lang, GetMessage("ParentSector", request.Lang), request.Data.ParentId.Value)
                            );
                        }

                        // Verify parent belongs to same company
                        if (parent.CompanyId != sector.CompanyId)
                        {
                            throw new DomainException(GetFormattedMessage("ParentMustBeSameCompany", request.Lang));
                        }

                        sector.UpdateParent(request.Data.ParentId);
                    }
                }

                await _repo.UpdateAsync(sector);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}