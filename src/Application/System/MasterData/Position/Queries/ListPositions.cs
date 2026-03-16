using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.System.MasterData.Position.Queries
{
    public static class ListPositions
    {
        public record Query(int Lang = 1) : IRequest<List<PositionDto>>;

        public class Handler : IRequestHandler<Query, List<PositionDto>>
        {
            private readonly IPositionRepository _repo;

            public Handler(IPositionRepository repo)
            {
                _repo = repo;
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
                    // هنا بنجيب الاسم بس من الـ ParentPosition
                    ParentPositionName: p.ParentPosition != null
                        ? (request.Lang == 2 ? p.ParentPosition.ArbName : p.ParentPosition.EngName)
                        : null,
                    PositionLevelId: p.PositionLevelId,
                    PositionLevelName: null,
                    EvalEvaluationId: p.EvalEvaluationId,
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