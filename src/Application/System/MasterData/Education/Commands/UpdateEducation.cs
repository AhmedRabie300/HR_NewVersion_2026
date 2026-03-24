using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Education.Dtos;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Education.Commands
{
    public static class UpdateEducation
    {
        public record Command(UpdateEducationDto Data, int Lang = 1) : IRequest<Unit>;

 

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IEducationRepository _repo;
            private readonly ILocalizationService _localizer;

            public Handler(IEducationRepository repo, ILocalizationService localizer)
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
                        _localizer.GetMessage("Education", request.Lang),
                        request.Data.Id));

                entity.Update(
                    request.Data.EngName,
                    request.Data.ArbName,
                    request.Data.ArbName4S,
                    request.Data.Level,
                    request.Data.RequiredYears,
                    request.Data.Remarks
                );

                await _repo.UpdateAsync(entity);
                await _repo.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}