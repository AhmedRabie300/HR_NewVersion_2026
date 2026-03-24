using Application.Common;
using Application.Common.Abstractions;
using Application.Common.BaseHandlers;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using Application.System.MasterData.Position.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Position.Commands
{
    public static class CreatePosition
    {
        public record Command(CreatePositionDto Data, int Lang = 1) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localization, int lang = 1)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new CreatePositionValidator(localization, lang));
            }
        }

        public class Handler : BaseCommandHandler, IRequestHandler<Command, int>
        {
            private readonly IPositionRepository _repo;

            public Handler(
                IPositionRepository repo,
                ILocalizationService localization) : base(localization)
            {
                _repo = repo;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                // Check if code exists
                var exists = await _repo.CodeExistsAsync(request.Data.Code);
                if (exists)
                {
                    throw new ConflictException(
                        GetMessage("Position", request.Lang),
                        GetMessage("Code", request.Lang),
                        request.Data.Code
                    );
                }

                // Check if parent position exists if provided
                if (request.Data.ParentId.HasValue)
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

                var position = new Domain.System.MasterData.Position(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    parentId: request.Data.ParentId,
                    positionLevelId: request.Data.PositionLevelId,
                    evalEvaluationId: request.Data.EvalEvaluationId,
                    evalRecruitmentId: request.Data.EvalRecruitmentId,
                    remarks: request.Data.Remarks,
                    employeesNo: request.Data.EmployeesNo,
                    applyValidation: request.Data.ApplyValidation,
                    positionBudget: request.Data.PositionBudget,
                    appraisalTypeGroupId: request.Data.AppraisalTypeGroupId,
                    regUserId: request.Data.RegUserId,
                    regComputerId: request.Data.regComputerId
                );

                await _repo.AddAsync(position);
                await _repo.SaveChangesAsync(cancellationToken);

                return position.Id;
            }
        }
    }
}