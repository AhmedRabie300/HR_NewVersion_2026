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
            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateContractTypeValidator(localizer, languageService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IContractTypeRepository _repo;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(IContractTypeRepository repo, ILanguageService languageService, ILocalizationService localizer)
            {
                _repo = repo;
                _languageService = languageService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _languageService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("ContractType", lang),
                        request.Data.Id));

                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.IsSpecial.HasValue ||
                    request.Data.Remarks != null)
                {
                    entity.Update(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.IsSpecial,
                        request.Data.Remarks
                    );
                }

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}