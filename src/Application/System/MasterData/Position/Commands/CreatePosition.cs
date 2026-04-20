using Application.Common;
using Application.Common.Abstractions;
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
        public record Command(CreatePositionDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IPositionRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreatePositionValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly IPositionRepository _repo;
                        private readonly IValidationMessages _msg;
public Handler(
                IPositionRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            { 

 

                var entity = new Domain.System.MasterData.Position(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    parentId: request.Data.ParentId,
                    positionLevelId: request.Data.PositionLevelId,
                    remarks: request.Data.Remarks,
                    employeesNo: request.Data.EmployeesNo,
                    applyValidation: request.Data.ApplyValidation,
                    positionBudget: request.Data.PositionBudget,
                    appraisalTypeGroupId: request.Data.AppraisalTypeGroupId
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}