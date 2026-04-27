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
    public static class AddGradeStepTransaction
    {
        public record Command(int GradeStepId, CreateGradeStepTransactionDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IGradeStepRepository repo)
            {
                RuleFor(x => x.GradeStepId)
                    .GreaterThan(0).WithMessage(x => msg.Get("GradeStepIdRequired"))
                    .MustAsync(async (id, cancellation) => await repo.ExistsAsync(id))
                    .WithMessage(x => msg.Format("NotFound", msg.Get("GradeStep"), x.GradeStepId));

                RuleFor(x => x.Data)
                    .SetValidator(new CreateGradeStepTransactionValidator(msg));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IGradeStepRepository _repo;
            private readonly ICurrentUser _currentUser;
            private readonly IValidationMessages _msg;

            public Handler(IGradeStepRepository repo, ICurrentUser currentUser, IValidationMessages msg)
            {
                _repo = repo;
                _currentUser = currentUser;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var gradeStep = await _repo.GetByIdAsync(request.GradeStepId);
                if (gradeStep == null)
                    throw new NotFoundException(_msg.NotFound("GradeStep", request.GradeStepId));

                var companyId = _currentUser.CompanyId;

                var entity = new GradeStepTransaction(
                    gradeStepId: request.GradeStepId,
                    gradeTransactionId: request.Data.GradeTransactionId,
                    companyId: companyId,
                    amount: request.Data.Amount,
                    remarks: request.Data.Remarks,
                    active: request.Data.Active,
                    activeDate: request.Data.ActiveDate,
                    activeDateD: request.Data.ActiveDateD,
                    regComputerId: request.Data.RegComputerId
                );

                await _repo.AddTransactionAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}