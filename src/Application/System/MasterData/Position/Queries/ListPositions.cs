using Application.Abstractions;
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using MediatR;

namespace Application.System.MasterData.Position.Queries
{
    public static class ListPositions
    {
        public record Query : IRequest<List<PositionDto>>;

        public class Handler : IRequestHandler<Query, List<PositionDto>>
        {
            private readonly IPositionRepository _repo;
            private readonly ICurrentUser _currentUser;

            public Handler(IPositionRepository repo, ICurrentUser currentUser)
            {
                _repo = repo;
                _currentUser = currentUser;
            }   
          

            public async Task<List<PositionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var positions = await _repo.GetAllAsync();

                return positions.Select(p => new PositionDto(
                    Id: p.Id,
                    Code: p.Code,
                    EngName: p.EngName,
                    ArbName: p.ArbName,
                    ArbName4S: p.ArbName4S,
                    ParentId: p.ParentId,
                    ParentPositionName: _currentUser.Language == 2 ? p.ParentPosition?.ArbName : p.ParentPosition?.EngName,
                    PositionLevelId: p.PositionLevelId,
                    PositionLevelName: null,
                    EvalEvaluationId: p.EvalEvaluationID,
                    EvalEvaluationName: null,
                    EvalRecruitmentId: p.EvalRecruitmentId,
                    EvalRecruitmentName: null,
                    Remarks: p.Remarks,
                    EmployeesNo: p.EmployeesNo,
                    ApplyValidation: p.ApplyValidation,
                    PositionBudget: p.PositionBudget,
                    AppraisalTypeGroupId: p.AppraisalTypeGroupId,
                    AppraisalTypeGroupName: null,
                    RegDate: p.RegDate,
                    CancelDate: p.CancelDate,
                    IsActive: p.IsActive()
                )).ToList();
            }
        }
    }
}