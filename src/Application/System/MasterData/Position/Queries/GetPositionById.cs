using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Position.Queries
{
    public static class GetPositionById
    {
        public record Query(int Id) : IRequest<PositionDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(IValidationMessages msg)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
            }
        }

        public class Handler : IRequestHandler<Query, PositionDto>
        {
            private readonly IPositionRepository _repo;
            private readonly IValidationMessages _msg;
            private readonly ICurrentUser _currentUser;

            public Handler(IPositionRepository repo, IValidationMessages msg, ICurrentUser currentUser)
            {
                _repo = repo;
                _msg = msg;
                _currentUser = currentUser;
            }

            public async Task<PositionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var position = await _repo.GetByIdAsync(request.Id);
                if (position == null)
                {
                    throw new NotFoundException(_msg.NotFound("Position", request.Id));
                }

                return new PositionDto(
                    Id: position.Id,
                    Code: position.Code,
                    EngName: position.EngName,
                    ArbName: position.ArbName,
                    ArbName4S: position.ArbName4S,
                    ParentId: position.ParentId,
                    ParentPositionName: _currentUser.Language == 2 ? position.ParentPosition?.ArbName : position.ParentPosition?.EngName,
                    PositionLevelId: position.PositionLevelId,
                    PositionLevelName: null,
                    EvalEvaluationId: position.EvalEvaluationID,
                    EvalEvaluationName: null,
                    EvalRecruitmentId: position.EvalRecruitmentId,
                    EvalRecruitmentName: null,
                    Remarks: position.Remarks,
                    EmployeesNo: position.EmployeesNo,
                    ApplyValidation: position.ApplyValidation,
                    PositionBudget: position.PositionBudget,
                    AppraisalTypeGroupId: position.AppraisalTypeGroupId,
                    AppraisalTypeGroupName: null,
                    RegDate: position.RegDate,
                    CancelDate: position.CancelDate,
                    IsActive: position.IsActive()
                );
            }
        }
    }
}