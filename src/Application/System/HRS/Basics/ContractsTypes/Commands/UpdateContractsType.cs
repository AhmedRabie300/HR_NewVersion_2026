using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.ContractsTypes.Dtos;
using Application.System.HRS.Basics.ContractsTypes.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.ContractsTypes.Commands
{
    public static class UpdateContractsType
    {
        public record Command(UpdateContractsTypeDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IContractsTypeRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateContractsTypeValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IContractsTypeRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IContractsTypeRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("ContractsType", request.Data.Id));

                entity.Update(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    isSpecial: request.Data.IsSpecial,
                    remarks: request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}