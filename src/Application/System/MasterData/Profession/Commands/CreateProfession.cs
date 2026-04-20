using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Profession.Dtos;
using Application.System.MasterData.Profession.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Profession.Commands
{
    public static class CreateProfession
    {
        public record Command(CreateProfessionDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IProfessionRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateProfessionValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IProfessionRepository _repo;
                        private readonly IValidationMessages _msg;
            private readonly IContextService _contextService;
public Handler(
                IProfessionRepository repo, IValidationMessages msg,
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
                    throw new ConflictException(_msg.CodeExists("Profession", request.Data.Code));
                }

var entity = new Domain.System.MasterData.Profession(
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