using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DependantType.Dtos;
using Application.System.MasterData.DependantType.Validators;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.DependantType.Commands
{
    public static class UpdateDependantType
    {
        public record Command(UpdateDependantTypeDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IDependantTypeRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateDependantTypeValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDependantTypeRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IDependantTypeRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("DependantType", request.Data.Id));

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}