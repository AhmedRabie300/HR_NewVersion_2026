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
            public Validator()
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateNationalityValidator());  
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly INationalityRepository _repo;

            public Handler(INationalityRepository repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var exists = await _repo.CodeExistsAsync(request.Data.Code);
                if (exists)
                    throw new Exception($"Nationality with code '{request.Data.Code}' already exists");

                var nationality = new Domain.System.MasterData.Nationality(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    isMainNationality: request.Data.IsMainNationality,
                    travelRoute: request.Data.TravelRoute,
                    travelClass: request.Data.TravelClass,
                    remarks: request.Data.Remarks,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.RegComputerId,
                    ticketAmount: request.Data.TicketAmount
                );

                await _repo.AddAsync(nationality);
                await _repo.SaveChangesAsync(cancellationToken);

                return nationality.Id;
            }
        }
    }
}