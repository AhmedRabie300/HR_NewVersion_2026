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
            private readonly ILanguageService _languageService;

            public Validator(ILocalizationService localizer, ILanguageService languageService)
            {
                _localizer = localizer;
                _languageService = languageService;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(x => _localizer.GetMessage("IdGreaterThanZero", _languageService.GetCurrentLanguage()));
            }
        }

        public class Handler : IRequestHandler<Query, PositionDto>
        {
            private readonly IPositionRepository _repo;
            private readonly ILanguageService _languageService;
            private readonly ILocalizationService _localizer;

            public Handler(IPositionRepository repo, ILanguageService languageService, ILocalizationService localizer)
            {
                _repo = repo;
                _languageService = languageService;
                _localizer = localizer;
            }

            public async Task<PositionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var lang = _languageService.GetCurrentLanguage();

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