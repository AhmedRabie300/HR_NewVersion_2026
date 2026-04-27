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
    public static class CreateGrade
    {
        public record Command(CreateGradeDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IGradeRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateGradeValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IGradeRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IGradeRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;

                var entity = new Grade(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    gradeLevel: request.Data.GradeLevel,
                    fromSalary: request.Data.FromSalary,
                    toSalary: request.Data.ToSalary,
                    regularHours: request.Data.RegularHours,
                    overTimeTypeId: request.Data.OverTimeTypeId,
                    companyId: companyId,
                    remarks: request.Data.Remarks,
                    regComputerId: request.Data.RegComputerId
                );

                // Add Transactions
                if (request.Data.Transactions != null)
                {
                    foreach (var transDto in request.Data.Transactions)
                    {
                        var transaction = new GradeTransaction(
                            gradeId: 0,
                            transactionTypeId: transDto.TransactionTypeId,
                            companyId: companyId,
                            minValue: transDto.MinValue,
                            maxValue: transDto.MaxValue,
                            paidAtVacation: transDto.PaidAtVacation,
                            onceAtPeriod: transDto.OnceAtPeriod,
                            intervalId: transDto.IntervalId,
                            numberOfTickets: transDto.NumberOfTickets,
                            remarks: transDto.Remarks,
                            regComputerId: transDto.RegComputerId
                        );
                        entity.AddTransaction(transaction);
                    }
                }

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}