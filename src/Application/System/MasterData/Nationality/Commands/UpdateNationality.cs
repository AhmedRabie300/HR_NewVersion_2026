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
            public Validator()
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateNationalityValidator());  // استخدام الـ Validator المنفصل
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly INationalityRepository _repo;

            public Handler(INationalityRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var nationality = await _repo.GetByIdAsync(request.Data.Id);
                if (nationality == null)
                    throw new Exception($"Nationality with ID {request.Data.Id} not found");

                // Update basic info
                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null)
                {
                    nationality.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );
                }

                // Update travel info
                if (request.Data.TravelRoute.HasValue ||
                    request.Data.TravelClass.HasValue ||
                    request.Data.TicketAmount.HasValue)
                {
                    nationality.UpdateTravelInfo(
                        request.Data.TravelRoute,
                        request.Data.TravelClass,
                        request.Data.TicketAmount
                    );
                }

                // Update main nationality status
                if (request.Data.IsMainNationality.HasValue)
                {
                    nationality.UpdateNationalityStatus(request.Data.IsMainNationality);
                }

                await _repo.UpdateAsync(nationality);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}