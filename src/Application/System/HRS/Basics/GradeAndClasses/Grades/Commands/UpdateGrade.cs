using Application.Common;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Dtos;
using Application.System.HRS.Basics.GradesAndClasses.Grades.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradesAndClasses.Grades.Commands
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

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}