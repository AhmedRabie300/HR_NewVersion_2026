        using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using FluentValidation;
using MediatR;

        namespace Application.System.MasterData.Sector.Commands
        {
            public static class SoftDeleteSector
            {
                public record Command(int Id) : IRequest<Unit>;

                public sealed class Validator : AbstractValidator<Command>
                {
                    public Validator(IValidationMessages msg)
                    {
                        RuleFor(x => x.Id).GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));
                    }
                }

                public class Handler : IRequestHandler<Command, Unit>
                {
                    private readonly ISectorRepository _repo;

                    public Handler(ISectorRepository repo)
                    {
                        _repo = repo;
                    }

                    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
                    {
                        await _repo.SoftDeleteAsync(request.Id);
                        await _repo.SaveChangesAsync(cancellationToken);

                        return Unit.Value;
                    }
                }
            }
        }
