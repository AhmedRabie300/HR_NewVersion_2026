using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.ContractType.Dtos;
using Application.System.MasterData.ContractType.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.ContractType.Commands
{
    public static class UpdateContractType
    {
        public record Command(UpdateContractTypeDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Validator(IContextService contextService, ILocalizationService localizer)
            {
                _contextService = contextService;
                _localizer = localizer;

                RuleFor(x => x.Data)
                    .SetValidator(new UpdateContractTypeValidator(_localizer, _contextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IContractTypeRepository _repo;
            private readonly IContextService _contextService;
            private readonly ILocalizationService _localizer;

            public Handler(
                IContractTypeRepository repo,
                IContextService contextService,
                ILocalizationService localizer)
            {
                _repo = repo;
                _contextService = contextService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _contextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(
                        _localizer.GetMessage("ContractType", lang),
                        request.Data.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("ContractType", lang), request.Data.Id));

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.IsSpecial,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}