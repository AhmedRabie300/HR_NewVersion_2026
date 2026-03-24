using Application.System.MasterData.Abstractions;
using Application.System.MasterData.MaritalStatus.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.MaritalStatus.Commands
{
    public static class UpdateMaritalStatus
    {
        public record Command(UpdateMaritalStatusDto Data, int Lang = 1) : IRequest<Unit>;

      
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IMaritalStatusRepository _repo;
            private readonly ILocalizationService _localizer;

            public Handler(IMaritalStatusRepository repo, ILocalizationService localizer)
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
                        _localizer.GetMessage("MaritalStatus", request.Lang),
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