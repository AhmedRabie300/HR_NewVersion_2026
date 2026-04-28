using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Validators;
using Domain.System.HRS.Basics.GradesAndClasses;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Commands
{
    public static class AddGradeTransaction
    {
        public record Command(int GradeId, CreateGradeTransactionDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IGradeRepository repo)
            {
            

                RuleFor(x => x.Data)
                    .SetValidator(new CreateGradeTransactionValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IGradeRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly IValidationMessages _msg;

            public Handler(IGradeRepository repo, ICurrentUser currentUser, IValidationMessages msg)
            {
                _repo = repo;
                _currentUser = currentUser;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var grade = await _repo.GetByIdAsync(request.GradeId);
                if (grade == null)
                    throw new NotFoundException(_msg.NotFound("Grade", request.GradeId));

                var companyId = _currentUser.CompanyId;

                var entity = new GradeTransaction(
                    gradeId: request.GradeId,
                    transactionTypeId: request.Data.TransactionTypeId,
                    companyId: companyId,
                    minValue: request.Data.MinValue,
                    maxValue: request.Data.MaxValue,
                    paidAtVacation: request.Data.PaidAtVacation,
                    onceAtPeriod: request.Data.OnceAtPeriod,
                    intervalId: request.Data.IntervalId,
                    numberOfTickets: request.Data.NumberOfTickets,
                    remarks: request.Data.Remarks,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddTransactionAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}