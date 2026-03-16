using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using Application.System.MasterData.Position.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Position.Commands
{
    public static class UpdatePosition
    {
        public record Command(UpdatePositionDto Data, int Lang = 1) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdatePositionValidator(localization, lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Command, Unit>
        {
            private readonly IPositionRepository _repo;

            public Handler(
                IPositionRepository repo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var position = await _repo.GetByIdAsync(request.Data.Id);
                if (position == null)
                {
                    throw new NotFoundException(
                        GetMessage("Position", request.Lang),
                        request.Data.Id,
                        GetFormattedMessage("NotFound", request.Lang, GetMessage("Position", request.Lang), request.Data.Id)
                    );
                }

                // Check if parent position exists if provided
                if (request.Data.ParentId.HasValue && request.Data.ParentId != position.Id)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                    {
                        throw new NotFoundException(
                            GetMessage("ParentPosition", request.Lang),
                            request.Data.ParentId.Value,
                            GetFormattedMessage("NotFound", request.Lang, GetMessage("ParentPosition", request.Lang), request.Data.ParentId.Value)
                        );
                    }
                }

                // Update basic info
                if (request.Data.EngName != null ||
                    request.Data.ArbName != null ||
                    request.Data.ArbName4S != null ||
                    request.Data.Remarks != null)
                {
                    position.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );
                }

                // Update relations
                if (request.Data.ParentId.HasValue ||
                    request.Data.PositionLevelId.HasValue ||
                    request.Data.EvalEvaluationId.HasValue ||
                    request.Data.EvalRecruitmentId.HasValue ||
                    request.Data.AppraisalTypeGroupId.HasValue)
                {
                    position.UpdateRelations(
                        request.Data.ParentId,
                        request.Data.PositionLevelId,
                        request.Data.EvalEvaluationId,
                        request.Data.EvalRecruitmentId,
                        request.Data.AppraisalTypeGroupId
                    );
                }

                // Update settings
                if (request.Data.EmployeesNo.HasValue ||
                    request.Data.ApplyValidation.HasValue ||
                    request.Data.PositionBudget != null)
                {
                    position.UpdateSettings(
                        request.Data.EmployeesNo,
                        request.Data.ApplyValidation,
                        request.Data.PositionBudget
                    );
                }

                await _repo.UpdateAsync(position);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}