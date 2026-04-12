using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Religion.Dtos;
using Application.System.MasterData.Religion.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Religion.Commands
{
    public static class CreateReligion
    {
        public record Command(CreateReligionDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateReligionValidator(localizer, ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IReligionRepository _repo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(IReligionRepository repo, IContextService ContextService, ILocalizationService localizer)
            {
                _repo = repo;
                _ContextService = ContextService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _ContextService.GetCurrentLanguage();

                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("Religion", lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.Religion(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.regComputerId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}