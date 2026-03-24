using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Religion.Dtos;
using Application.System.MasterData.Religion.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Religion.Commands
{
    public static class UpdateReligion
    {
        public record Command(UpdateReligionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Data).SetValidator(new UpdateReligionValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IReligionRepository _repo;

            public Handler(IReligionRepository repo)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var religion = await _repo.GetByIdAsync(request.Data.Id);
                if (religion == null)
                    throw new Exception($"Religion with ID {request.Data.Id} not found");

                religion.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(religion);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}