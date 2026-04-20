using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Nationality.Dtos;
using Application.System.MasterData.Nationality.Validators;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Nationality.Commands
{
    public static class UpdateNationality
    {
        public record Command(UpdateNationalityDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, INationalityRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateNationalityValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly INationalityRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(INationalityRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Nationality", request.Data.Id));

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