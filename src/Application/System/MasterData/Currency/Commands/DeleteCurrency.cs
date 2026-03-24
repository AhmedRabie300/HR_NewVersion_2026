using Application.System.MasterData.Abstractions;
using Application.Common.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.MasterData.Currency.Commands
{
    public static class DeleteCurrency
    {
        public record Command(int Id, int Lang = 1) : IRequest<bool>;

        public sealed class Validator : AbstractValidator<Command>
        {
            public Validator(ILocalizationService localizer)
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(localizer.GetMessage("IdGreaterThanZero", 1));
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly ICurrencyRepository _repo;

            public Handler(ICurrencyRepository repo)
            {
                _repo = repo;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await _repo.ExistsAsync(request.Id)) return false;

                await _repo.DeleteAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}