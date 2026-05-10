using Application.Common;
using Application.Common.Abstractions;
using Application.System.MasterData.Abstractions;
using FluentValidation;
using MediatR;
using Application.Abstractions;

namespace Application.System.MasterData.Nationality.Commands
{
    public static class SoftDeleteNationality
    {
        public record Command(int Id) : IRequest<Unit>;
         public sealed class Validator : AbstractValidator<Command>
        {
            private readonly INationalityRepository _repo;   

            public Validator(IValidationMessages msg, INationalityRepository repo)
            {
                _repo = repo;

                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"));



                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(msg.Get("IdGreaterThanZero"))
                     .MustAsync(async (id, cancellation) => !await _repo.IsUsedInEmployeesAsync(id))
                    .WithMessage(msg.Get("NationalityUsedInEmployees"))
                   .MustAsync(async (id, cancellation) =>
                    {
                        var nationality = await _repo.GetByIdAsync(id);
                        if (nationality == null) return true;
                        return !(nationality.IsMainNationality == true);
                    })
                    .WithMessage(msg.Get("CannotDeleteMainNationality"));
            





        }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly INationalityRepository _repo;

            public Handler(INationalityRepository repo)
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