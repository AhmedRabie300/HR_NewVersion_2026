using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Validators;

using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Commands
{
    public static class UpdateGradeStep
    {
        public record Command(UpdateGradeStepDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IGradeStepRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateGradeStepValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IGradeStepRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IGradeStepRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("GradeStep", request.Data.Id));

                entity.Update(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    step: request.Data.Step,
                    remarks: request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}