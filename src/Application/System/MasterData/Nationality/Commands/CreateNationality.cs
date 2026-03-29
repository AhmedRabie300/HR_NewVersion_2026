using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Nationality.Dtos;
using Application.System.MasterData.Nationality.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Nationality.Commands
{
    public static class CreateNationality
    {
        public record Command(CreateNationalityDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILanguageService languageService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateNationalityValidator(localizer, languageService));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly INationalityRepository _repo;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(INationalityRepository repo, ILanguageService languageService, ILocalizationService localizer)
            {
                _repo = repo;
                _languageService = languageService;
                _localizer = localizer;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _languageService.GetCurrentLanguage();

                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new ConflictException(string.Format(
                        _localizer.GetMessage("CodeExists", lang),
                        _localizer.GetMessage("Nationality", lang),
                        request.Data.Code));

                var entity = new Domain.System.MasterData.Nationality(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.IsMainNationality,
                    request.Data.TravelRoute,
                    request.Data.TravelClass,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.regComputerId,
                    request.Data.TicketAmount
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}