using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Dtos;
using Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Validators;

using Domain.System.HRS.Basics.GradesAndClasses;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Commands
{
    public static class CreateGradeStep
    {
        public record Command(CreateGradeStepDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IGradeStepRepository repo)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreateGradeStepValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IGradeStepRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IGradeStepRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var companyId = _currentUser.CompanyId;

                var entity = new GradeStep(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    gradeId: request.Data.GradeId,
                    step: request.Data.Step,
                    companyId: companyId,
                    remarks: request.Data.Remarks,
                    regComputerId: request.Data.RegComputerId
                );

                // Add Transactions
                if (request.Data.Transactions != null)
                {
                    foreach (var transDto in request.Data.Transactions)
                    {
                        var transaction = new GradeStepTransaction(
                            gradeStepId: 0,
                            gradeTransactionId: transDto.GradeTransactionId,
                            companyId: companyId,
                            amount: transDto.Amount,
                            remarks: transDto.Remarks,
                            active: transDto.Active,
                            activeDate: transDto.ActiveDate,
                            activeDateD: transDto.ActiveDateD,
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