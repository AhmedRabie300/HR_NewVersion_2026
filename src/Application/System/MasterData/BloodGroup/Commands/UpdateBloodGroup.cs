using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.BloodGroup.Dtos;
using Application.System.MasterData.BloodGroup.Validators;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.BloodGroup.Commands
{
    public static class UpdateBloodGroup
    {
        public record Command(UpdateBloodGroupDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IBloodGroupRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateBloodGroupValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IBloodGroupRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IBloodGroupRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var bloodGroup = await _repo.GetByIdAsync(request.Data.Id);
                if (bloodGroup == null)
                    throw new NotFoundException(_msg.NotFound("BloodGroup", request.Data.Id));

                bloodGroup.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(bloodGroup);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}