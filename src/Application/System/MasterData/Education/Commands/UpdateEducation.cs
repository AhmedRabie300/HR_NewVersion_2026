using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using Application.System.MasterData.Education.Dtos;
using Application.System.MasterData.Education.Validators;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Education.Commands
{
    public static class UpdateEducation
    {
        public record Command(UpdateEducationDto Data) : IRequest<Unit>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(IValidationMessages msg, IEducationRepository repo)
            {
                RuleFor(x => x.Data).SetValidator(new UpdateEducationValidator(msg, repo));
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IEducationRepository _repo;
            private readonly IValidationMessages _msg;

            public Handler(IEducationRepository repo, IValidationMessages msg)
            {
                _repo = repo;
                _msg = msg;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = await _repo.GetByIdAsync(request.Data.Id);
                if (entity == null)
                    throw new NotFoundException(_msg.NotFound("Education", request.Data.Id));

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