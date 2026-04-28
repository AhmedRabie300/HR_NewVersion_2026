using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Validators;

using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Commands
{
    public static class UpdateGradeStepTransaction
    {
        public record Command(UpdateGradeStepTransactionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IGradeStepRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateGradeStepTransactionValidator(msg));
 
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
                var entity = await _repo.GetTransactionByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("GradeStepTransaction", request.Data.Id));

                entity.Update(
                    amount: request.Data.Amount,
                    remarks: request.Data.Remarks,
                    active: request.Data.Active,
                    activeDate: request.Data.ActiveDate,
                    activeDateD: request.Data.ActiveDateD
                );

                await _repo.UpdateTransactionAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}