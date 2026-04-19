using Application.Common;
using Application.Common.Abstractions;
using Application.UARbac.Abstractions;
using Application.UARbac.Modules.Dtos;
using Application.UARbac.Modules.Validators;
using Domain.UARbac;
using FluentValidation;
using MediatR;

namespace Application.UARbac.Modules.Commands
{
    public static class CreateModule
    {
        public record Command(CreateModuleDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateModuleValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IModuleRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IModuleRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var exists = await _repo.CodeExistsAsync(request.Data.Code);
                if (exists)
                    throw new ConflictException(_msg.CodeExists("Module", request.Data.Code));

                var module = new Module(
                    code: request.Data.Code,
                    prefix: request.Data.Prefix,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    formId: request.Data.FormId,
                    isRegistered: request.Data.IsRegistered,
                    fiscalYearDependant: request.Data.FiscalYearDependant,
                    rank: request.Data.Rank,
                    remarks: request.Data.Remarks,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.regComputerId
                );

                await _repo.AddAsync(module);
                await _repo.SaveChangesAsync(cancellationToken);

                return module.Id;
            }
        }
    }
}
