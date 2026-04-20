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
            public Validator(IValidationMessages msg,IReligionRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateReligionValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IReligionRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(
                IReligionRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
        

var entity = new Domain.System.MasterData.Religion(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    remarks: request.Data.Remarks
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}