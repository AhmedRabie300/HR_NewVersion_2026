using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.ContractType.Dtos;
using Application.System.MasterData.ContractType.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.ContractType.Commands
{
    public static class CreateContractType
    {
        public record Command(CreateContractTypeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,IContractTypeRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateContractTypeValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IContractTypeRepository _repo;
            private readonly IValidationMessages _msg;
            public Handler(IContractTypeRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {


                var codeExists = await _repo.CodeExistsAsync(request.Data.Code);
                if (codeExists)
                {
                    throw new ConflictException(_msg.CodeExists("ContractType", request.Data.Code));
                }

                    var entity = new Domain.System.MasterData.ContractType(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    isSpecial: request.Data.IsSpecial,
                    remarks: request.Data.Remarks
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}