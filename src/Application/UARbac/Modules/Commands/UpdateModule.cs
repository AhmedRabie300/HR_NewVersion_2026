using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Modules.Dtos;
using Application.UARbac.Abstractions;
using FluentValidation;
using MediatR;
using Application.UARbac.Modules.Validators;

namespace Application.UARbac.Modules.Commands
{
    public static class UpdateModule
    {
        public record Command(UpdateModuleDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new UpdateModuleValidator(_contextService, _localizer));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IModuleRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(IModuleRepository repo, IContextService contextService, ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var module = await _repo.GetByIdAsync(request.Data.Id);
                if (module == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("Module", lang),
                        request.Data.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Module", lang), request.Data.Id));

                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Rank.HasValue ||
                    request.Data.Remarks != null)
                {
                    module.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Rank,
                        request.Data.Remarks
                    );
                }

                if (request.Data.IsAR.HasValue ||
                    request.Data.IsAP.HasValue ||
                    request.Data.IsGL.HasValue ||
                    request.Data.IsFA.HasValue ||
                    request.Data.IsINV.HasValue ||
                    request.Data.IsHR.HasValue ||
                    request.Data.IsMANF.HasValue ||
                    request.Data.IsSYS.HasValue)
                {
                    module.UpdateModuleTypes(
                        request.Data.IsAR,
                        request.Data.IsAP,
                        request.Data.IsGL,
                        request.Data.IsFA,
                        request.Data.IsINV,
                        request.Data.IsHR,
                        request.Data.IsMANF,
                        request.Data.IsSYS
                    );
                }

                if (request.Data.IsRegistered.HasValue ||
                    request.Data.FiscalYearDependant.HasValue)
                {
                    module.UpdateRegistration(
                        request.Data.IsRegistered,
                        request.Data.FiscalYearDependant
                    );
                }

                if (request.Data.FormId.HasValue)
                {
                    module.UpdateForm(request.Data.FormId);
                }

                await _repo.UpdateAsync(module);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}