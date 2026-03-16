using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Position.Queries
{
    public static class GetPositionById
    {
        public record Query(int Id, int Lang = 1) : IRequest<PositionDto>;

        public sealed class Validator : AbstractValidator<Query>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(localization.GetMessage("IdGreaterThanZero", lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Query, PositionDto>
        {
            private readonly IPositionRepository _repo;

            public Handler(
                IPositionRepository repo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
            }

            public async Task<PositionDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var position = await _repo.GetByIdAsync(request.Id);
                if (position == null)
                {
                    throw new NotFoundException(
                        GetMessage("Position", request.Lang),
                        request.Id,
                        GetFormattedMessage("NotFound", request.Lang, GetMessage("Position", request.Lang), request.Id)
                    );
                }

                return new PositionDto(
                    Id: position.Id,
                    Code: position.Code,
                    EngName: position.EngName,
                    ArbName: position.ArbName,
                    ArbName4S: position.ArbName4S,
                    ParentId: position.ParentId,
                    ParentPositionName: request.Lang == 2 ? position.ParentPosition?.ArbName : position.ParentPosition?.EngName,
                    PositionLevelId: position.PositionLevelId,
                    PositionLevelName: null, // هيتم ملئها بعدين
                    EvalEvaluationId: position.EvalEvaluationId,
                    EvalEvaluationName: null, // هيتم ملئها بعدين
                    EvalRecruitmentId: position.EvalRecruitmentId,
                    EvalRecruitmentName: null, // هيتم ملئها بعدين
                    Remarks: position.Remarks,
                    EmployeesNo: position.EmployeesNo,
                    ApplyValidation: position.ApplyValidation,
                    PositionBudget: position.PositionBudget,
                    AppraisalTypeGroupId: position.AppraisalTypeGroupId,
                    AppraisalTypeGroupName: null, // هيتم ملئها بعدين
                    RegDate: position.RegDate,
                    CancelDate: position.CancelDate,
                    IsActive: position.IsActive()
                );
            }
        }
    }
}