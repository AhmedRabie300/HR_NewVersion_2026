using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using Application.Common.Models;
using MediatR;

namespace Application.System.MasterData.Position.Queries
{
    public static class GetPagedPositions
    {
        public record Query(int PageNumber, int PageSize, string? SearchTerm) : IRequest<PagedResult<PositionDto>>;

        public class Handler : IRequestHandler<Query, PagedResult<PositionDto>>
        {
            private readonly IPositionRepository _repo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(IPositionRepository repo, IContextService ContextService, ILocalizationService localizer)
            {
                _repo = repo;
                _ContextService = ContextService;
                _localizer = localizer;
            }

            public async Task<PagedResult<PositionDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _ContextService.GetCurrentLanguage();
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
                    ParentPositionName: lang == 2 ? p.ParentPosition?.ArbName : p.ParentPosition?.EngName,
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