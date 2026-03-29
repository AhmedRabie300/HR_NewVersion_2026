using Application.Common;
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
            public Validator()
            {
                RuleFor(x => x.Data).SetValidator(new CreateReligionValidator());
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IReligionRepository _repo;

            public Handler(IReligionRepository repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _repo.CodeExistsAsync(request.Data.Code))
                    throw new ConflictException($"Religion with code '{request.Data.Code}' already exists");

                var religion = new Domain.System.MasterData.Religion(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks,
                    request.Data.RegUserId,
                    request.Data.regComputerId
                );

                await _repo.AddAsync(religion);
                await _repo.SaveChangesAsync(cancellationToken);

                return religion.Id;
            }
        }
    }
}