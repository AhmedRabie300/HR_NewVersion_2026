using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DependantType.Dtos;
using Application.System.MasterData.DependantType.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.DependantType.Commands
{
    public static class CreateDependantType
    {
        public record Command(CreateDependantTypeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IDependantTypeRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateDependantTypeValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IDependantTypeRepository _repo;
            private readonly IValidationMessages _msg;
        public Handler(
                IDependantTypeRepository repo, IValidationMessages msg,
                IContextService contextService)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var codeExists = await _repo.CodeExistsAsync(request.Data.Code);
                if (codeExists)
                {
                    throw new ConflictException(_msg.CodeExists("DependantType", request.Data.Code));
                }

var entity = new Domain.System.MasterData.DependantType(
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