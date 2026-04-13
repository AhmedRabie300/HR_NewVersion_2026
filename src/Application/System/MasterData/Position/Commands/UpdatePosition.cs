using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Position.Dtos;
using Application.System.MasterData.Position.Validators;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Position.Commands
{
    public static class UpdatePosition
    {
        public record Command(UpdatePositionDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IContextService ContextService, ILocalizationService localizer)
            {
                RuleFor(x => x.Data)
                    .SetValidator(new UpdatePositionValidator(localizer, ContextService));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IPositionRepository _repo;
            private readonly ICompanyRepository _companyRepo;
            private readonly IContextService _ContextService;
            private readonly ILocalizationService _localizer;

            public Handler(IPositionRepository repo, ICompanyRepository companyRepo, IContextService ContextService, ILocalizationService localizer)
            {
                _repo = repo;
                _companyRepo = companyRepo;
                _ContextService = ContextService;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var lang = _ContextService.GetCurrentLanguage();

                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException("NotFound", string.Format(
                        _localizer.GetMessage("NotFound", lang),
                        _localizer.GetMessage("Position", lang),
                        request.Data.Id));

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
                        throw new NotFoundException("NotFound", string.Format(
                            _localizer.GetMessage("NotFound", lang),
                            _localizer.GetMessage("ParentPosition", lang),
                            request.Data.ParentId));
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