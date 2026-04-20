// Application/System/MasterData/Sector/Commands/UpdateSector.cs
using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using Application.System.MasterData.Sector.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Abstractions;

namespace Application.System.MasterData.Sector.Commands
{
    public static class UpdateSector
    {
        public record Command(UpdateSectorDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,ISectorRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateSectorValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly ISectorRepository _repo;
                        private readonly IValidationMessages _msg;
            private readonly IContextService _ContextService;
public Handler(
                ISectorRepository repo, IValidationMessages msg )
            {
                _repo = repo;
                _msg = msg;
                
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                 var entity = await _repo.GetByIdAsync(request.Data.Id);
             
 
                    entity.UpdateBasicInfo(
                        request.Data.EngName,
                        request.Data.ArbName,
                        request.Data.ArbName4S,
                        request.Data.Remarks
                    );

                 if (request.Data.ParentId.HasValue && request.Data.ParentId != entity.Id)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new NotFoundException(_msg.NotFound("ParentSector", request.Data.ParentId));
                    entity.UpdateParent(request.Data.ParentId);
                }

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}