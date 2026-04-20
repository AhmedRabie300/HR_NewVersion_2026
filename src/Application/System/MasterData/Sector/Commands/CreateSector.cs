using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Sector.Dtos;
using Application.System.MasterData.Sector.Validators;
using Domain.System.MasterData;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Sector.Commands
{
    public static class CreateSector
    {
        public record Command(CreateSectorDto Data) : IRequest<int>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg,ISectorRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new CreateSectorValidator(msg,repo));
            }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly ISectorRepository _repo;
                        private readonly IValidationMessages _msg;
            private readonly IContextService _contextService;
public Handler(
                ISectorRepository repo, IValidationMessages msg,
                IContextService contextService)
            {
                _repo = repo;
                _msg = msg;
                _contextService = contextService;
                _contextService = contextService;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
 
            

if (request.Data.ParentId.HasValue)
                {
                    var parent = await _repo.GetByIdAsync(request.Data.ParentId.Value);
                    if (parent == null)
                        throw new NotFoundException(_msg.NotFound("ParentSector", request.Data.ParentId.Value));
                }

                var entity = new Domain.System.MasterData.Sector(
                    code: request.Data.Code,
                    engName: request.Data.EngName,
                    arbName: request.Data.ArbName,
                    arbName4S: request.Data.ArbName4S,
                    parentId: request.Data.ParentId,
                    remarks: request.Data.Remarks
                );

                await _repo.AddAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}