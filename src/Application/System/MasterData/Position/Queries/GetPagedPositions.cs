using Application.Common.Models;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using MediatR;

namespace Application.System.MasterData.Position.Queries
{
    public static class GetPagedPositions
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm, int Lang = 1) : IRequest<PagedResult<PositionDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<PositionDto>>
        {
            private readonly IPositionRepository _repo;

            public Handler(IPositionRepository repo)
            {
                _repo = repo;
            }

            public async Task<PagedResult<PositionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var pagedResult = await _repo.GetPagedAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm
                );

                var items = pagedResult.Items.Select(p => new PositionDto(
                    Id: p.Id,
                    Code: p.Code,
                    EngName: p.EngName,
                    ArbName: p.ArbName,
                    ArbName4S: p.ArbName4S,
                    ParentId: p.ParentId,
                    ParentPositionName: request.Lang == 2 ? p.ParentPosition?.ArbName : p.ParentPosition?.EngName,
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

                return new PagedResult<PositionDto>(
                    items,
                    pagedResult.PageNumber,
                    pagedResult.PageSize,
                    pagedResult.TotalCount
                );
            }
        }
    }
}