using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Commands
{
    public static class UpdateGradeTransaction
    {
        public record Command(UpdateGradeTransactionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IGradeRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateGradeTransactionValidator(msg));

                RuleFor(x => x.Data.GradeId)
                    .MustAsync(async (id, cancellation) => await repo.ExistsAsync(id))
                    .WithMessage(x => msg.Format("NotFound", msg.Get("Grade"), x.Data.GradeId));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IGradeRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IGradeRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetTransactionByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("GradeTransaction", request.Data.Id));

                entity.Update(
                    minValue: request.Data.MinValue,
                    maxValue: request.Data.MaxValue,
                    paidAtVacation: request.Data.PaidAtVacation,
                    onceAtPeriod: request.Data.OnceAtPeriod,
                    intervalId: request.Data.IntervalId,
                    numberOfTickets: request.Data.NumberOfTickets,
                    remarks: request.Data.Remarks
                );

                await _repo.UpdateTransactionAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}