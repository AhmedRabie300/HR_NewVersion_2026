// Application/System/MasterData/Sector/Commands/UpdateSector.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using Application.System.MasterData.Sector.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.System.MasterData.Sector.Commands
{
    public static class UpdateSector
    {
        public record Command(UpdateSectorDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateSectorValidator(_localizer, _ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ISectorRepository _repo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                ISectorRepository repo,
                IContextService ContextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _ContextService = ContextService;
                _localizer = localizer;
            }



            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _ContextService.GetCurrentCompanyId();
                var lang = _ContextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Sector", lang),
                        request.Data.Id));

                // التأكد أن القطاع يتبع الشركة الحالية
                if (entity.CompanyId != companyId)
                    throw new UnauthorizedAccessException("Access denied: Sector does not belong to your company");

                // Update basic info
                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null)
                {
                    entity.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );
                }

                // Update parent
                if (request.Data.ParentId.HasValue && request.Data.ParentId != entity.Id)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new Exception(string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("ParentSector", lang),
                            request.Data.ParentId));
                    entity.UpdateParent(request.Data.ParentId);
                }

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}