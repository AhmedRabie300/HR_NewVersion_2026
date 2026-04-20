using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sponsor.Dtos;
using Application.System.MasterData.Sponsor.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Sponsor.Commands
{
    public static class CreateSponsor
    {
        public record Command(CreateSponsorDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,ISponsorRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateSponsorValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ISponsorRepository _repo;
                        private readonly IValidationMessages _msg;
            private readonly IContextService _contextService;
public Handler(
                ISponsorRepository repo, IValidationMessages msg,
                IContextService contextService)
            {
                _repo = repo;
                _msg = msg;
                _contextService = contextService;
                _contextService = contextService;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
 
                var codeExists = await _repo.CodeExistsAsync(request.Data.Code);
                if (codeExists)
                {
                    throw new ConflictException(_msg.CodeExists("Sponsor", request.Data.Code));
                }

var entity = new Domain.System.MasterData.Sponsor(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    sponsorNumber: request.Data.SponsorNumber
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}