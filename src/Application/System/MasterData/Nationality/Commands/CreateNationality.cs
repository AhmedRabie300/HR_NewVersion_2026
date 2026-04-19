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
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data).SetValidator(new CreateNationalityValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly INationalityRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(
                INationalityRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var codeExists = await _repo.CodeExistsAsync(request.Data.Code);
                if (codeExists)
                {
                    throw new ConflictException(_msg.CodeExists("Nationality", request.Data.Code));
                }

var entity = new Domain.System.MasterData.Nationality(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    isMainNationality: request.Data.IsMainNationality,
                    travelRoute: request.Data.TravelRoute,
                    travelClass: request.Data.TravelClass,
                    remarks: request.Data.Remarks,
                    ticketAmount: request.Data.TicketAmount
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}