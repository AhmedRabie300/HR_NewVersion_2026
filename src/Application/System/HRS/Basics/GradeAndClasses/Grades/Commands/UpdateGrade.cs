using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Validators;
using Domain.System.HRS.Basics.GradesAndClasses;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.Grades.Commands
{
    public static class UpdateGrade
    {
        public record Command(UpdateGradeDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IGradeRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdateGradeValidator(msg, repo));
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
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Grade", request.Data.Id));

                // Update Grade (Header)
                entity.Update(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    gradeLevel: request.Data.GradeLevel,
                    fromSalary: request.Data.FromSalary,
                    toSalary: request.Data.ToSalary,
                    regularHours: request.Data.RegularHours,
                    overTimeTypeId: request.Data.OverTimeTypeId,
                    remarks: request.Data.Remarks
                );

                 if (request.Data.Transactions != null)
                {
                    foreach (var transDto in request.Data.Transactions)
                    {
                         var transaction = entity.Transactions.FirstOrDefault(t => t.Id == transDto.Id);
                        if (transaction != null)
                        {
                            transaction.Update(
                                minValue: transDto.MinValue,
                                maxValue: transDto.MaxValue,
                                paidAtVacation: transDto.PaidAtVacation,
                                onceAtPeriod: transDto.OnceAtPeriod,
                                intervalId: transDto.IntervalId,
                                numberOfTickets: transDto.NumberOfTickets,
                                remarks: transDto.Remarks
                            );
                        }
                        else
                        {
                             var newTransaction = new GradeTransaction(
                                gradeId: entity.Id,
                                transactionTypeId: transDto.TransactionTypeId,
                                 minValue: transDto.MinValue,
                                maxValue: transDto.MaxValue,
                                paidAtVacation: transDto.PaidAtVacation,
                                onceAtPeriod: transDto.OnceAtPeriod,
                                intervalId: transDto.IntervalId,
                                numberOfTickets: transDto.NumberOfTickets,
                                remarks: transDto.Remarks
                             );
                            entity.AddTransaction(newTransaction);
                        }
                    }
                }

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}