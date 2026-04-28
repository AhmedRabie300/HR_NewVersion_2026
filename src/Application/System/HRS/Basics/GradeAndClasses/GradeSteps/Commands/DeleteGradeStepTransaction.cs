using Application.Abstractions;
using Application.Common.Abstractions;
using Application.System.HRS.Abstractions;
using FluentValidation;
using MediatR;

namespace Application.System.HRS.Basics.GradeAndClasses.GradeSteps.Commands
{
    public static class DeleteGradeStepTransaction
    {
        public record Command(int Id) : IRequest<bool>;

        public sealed class Validator : AbstractValidator<Command>
        {
            private readonly ICurrentUser _CurrentUser;
            private readonly ILocalizationService _localizer;

            public Validator(ICurrentUser CurrentUser, ILocalizationService localizer)
            {
                _CurrentUser = CurrentUser;
                _localizer = localizer;
                var lang = _CurrentUser.Language;
                RuleFor(x => x.Id)
                    .GreaterThan(0).WithMessage(_localizer.GetMessage("IdGreaterThanZero", lang));
            }
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly IGradeStepRepository _repo;

            public Handler(IGradeStepRepository repo)
            {
                _repo = repo;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await _repo.TransactionExistsAsync(request.Id))
                    return false;

                await _repo.DeleteTransactionAsync(request.Id);
                await _repo.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}