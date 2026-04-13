using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Modules.Dtos;
using Application.UARbac.Abstractions;
using Domain.UARbac;
using FluentValidation;
using MediatR;
using Application.UARbac.Modules.Validators;

namespace Application.UARbac.Modules.Commands
{
    public static class CreateModule
    {
        public record Command(CreateModuleDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new CreateModuleValidator(_contextService, _localizer));
            }
        }

        public class Handler : IRequestHandler<Command, int>
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

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var exists = await _repo.CodeExistsAsync(request.Data.Code);
                if (exists)
                    throw new ConflictException(
                        _localizer.GetMessage("Module", lang),
                        "Code",
                        request.Data.Code,
                        string.Format(_localizer.GetMessage("CodeExists", lang), _localizer.GetMessage("Module", lang), request.Data.Code));

                var module = new Module(
                    code: request.Data.Code,
                    prefix: request.Data.Prefix,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    formId: request.Data.FormId,
                    isRegistered: request.Data.IsRegistered,
                    fiscalYearDependant: request.Data.FiscalYearDependant,
                    rank: request.Data.Rank,
                    remarks: request.Data.Remarks,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.regComputerId
                );

                await _repo.AddAsync(module);
                await _repo.SaveChangesAsync(cancellationToken);

                return module.Id;
            }
        }
    }
}