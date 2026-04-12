using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Position.Queries
{
    public static class GetPositionById
    {
        public record Query(int Id) : IRequest<PositionDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            private readonly ILocalizationService _localizer;
            private readonly IContextService _ContextService;

            public Validator(ILocalizationService localizer, IContextService ContextService)
            {
                _localizer = localizer;
                _ContextService = ContextService;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _ContextService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, PositionDto>
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

            public async Task<PositionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _ContextService.GetCurrentLanguage();

                var position = await _repo.GetByIdAsync(request.Id);
                if (position == null)
                {
                    throw new NotFoundException(
                        _localizer.GetMessage("Position", lang),
                        request.Id,
                        string.Format(_localizer.GetMessage("NotFound", lang), _localizer.GetMessage("Position", lang), request.Id)
                    );
                }

                return new PositionDto(
                    Id: position.Id,
                    Code: position.Code,
                    EngName: position.EngName,
                    ArbName: position.ArbName,
                    ArbName4S: position.ArbName4S,
                    ParentId: position.ParentId,
                    ParentPositionName: lang == 2 ? position.ParentPosition?.ArbName : position.ParentPosition?.EngName,
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