using Application.System.MasterData.Abstractions;
using Application.System.MasterData.DependantType.Dtos;
using Application.System.MasterData.DependantType.Validators;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.DependantType.Commands
{
    public static class UpdateDependantType
    {
        public record Command(UpdateDependantTypeDto Data, int Lang = 1) : IRequest<Unit>;

 

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IDependantTypeRepository _repo;
            private readonly ILocalizationService _localizer;

            public Handler(IDependantTypeRepository repo, ILocalizationService localizer)
            {
                _repo = repo;
                _localizer = localizer;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new Exception(string.Format(
                        _localizer.GetMessage("NotFound", request.Lang),
                        _localizer.GetMessage("DependantType", request.Lang),
                        request.Data.Id));

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}