using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using Application.System.MasterData.Position.Validators;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Position.Commands
{
    public static class UpdatePosition
    {
        public record Command(UpdatePositionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,IPositionRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new UpdatePositionValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IPositionRepository _repo;
                        private readonly IValidationMessages _msg;
private readonly ICompanyRepository _companyRepo;
            public Handler(IPositionRepository repo, IValidationMessages msg, ICompanyRepository companyRepo)
            {
                _repo = repo;
                _msg = msg;
                _companyRepo = companyRepo;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Position", request.Data.Id));

                // Update basic info

                    entity.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );

                // Update parent
                if (request.Data.ParentId.HasValue && request.Data.ParentId != entity.Id)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new NotFoundException(_msg.NotFound("ParentPosition", request.Data.ParentId));
                    entity.UpdateParent(request.Data.ParentId);
                }

                    entity.UpdatePositionLevel(request.Data.PositionLevelId);

                    entity.UpdateEmployeesNo(request.Data.EmployeesNo);

                    entity.UpdatePositionBudget(request.Data.PositionBudget);

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}