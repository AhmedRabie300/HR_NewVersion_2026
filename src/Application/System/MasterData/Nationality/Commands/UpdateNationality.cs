using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Nationality.Dtos;
using Application.System.MasterData.Nationality.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Nationality.Commands
{
    public static class UpdateNationality
    {
        public record Command(UpdateNationalityDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateNationalityValidator(localizer, ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly INationalityRepository _repo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(INationalityRepository repo, IContextService ContextService, ILocalizationService localizer)
            {
                _repo = repo;
                _ContextService = ContextService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _ContextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Nationality", lang),
                        request.Data.Id));

                // Update basic info
               
                    entity.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );
                

             
                    entity.UpdateTravelInfo(
                        request.Data.TravelRoute,
                        request.Data.TravelClass,
                        request.Data.TicketAmount
                    );
                 

                // Update main nationality status
                if (request.Data.IsMainNationality.HasValue)
                {
                    entity.UpdateNationalityStatus(request.Data.IsMainNationality);
                }

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}