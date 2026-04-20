using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Gender.Dtos;
using Application.System.HRS.Gender.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Gender.Commands
{
    public static class UpdateGender
    {
        public record Command(UpdateGenderDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IGenderRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateGenderValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IGenderRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(
                IGenderRepository repo,
                IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity is null)
                    throw new NotFoundException(_msg.NotFound("Gender", request.Data.Id));

                entity.Update(
                    request.Data.Code,
                    request.Data.EngName,
                    request.Data.ArbName
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}